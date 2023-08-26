using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpendingTracker.Application;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure;
using SpendingTracker.TelegramBot;
using SpendingTracker.TelegramBot.Internal;
using SpendingTracker.TelegramBot.Internal.Abstractions;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.Model;
using SpendingTracker.TelegramBot.SpendingParsing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient("6133107700:AAFPgfpteJtzLfauIHkmobDp8JbNNxrIwm0");
Console.OutputEncoding = Encoding.UTF8;
var serviceProvider = InitializeDependencies();
var gatewayService = serviceProvider.GetService<GatewayService>()!;
var telegramUserCurrentButtonGroupService = serviceProvider.GetService<ITelegramUserCurrentButtonGroupService>()!;
var telegramUserIdStore = serviceProvider.GetService<ITelegramUserIdStore>()!;
var spendingMessageParser = new SpendingMessageParser();
var buttonsGroupManager = serviceProvider.GetService<IButtonsGroupManager>()!;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

var cts = new CancellationTokenSource();
var defaultCancellationToken = cts.Token;
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }, // receive all update types
};
bot.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    defaultCancellationToken
);
Console.ReadLine();
return;

async Task HandleUpdateAsync(
    ITelegramBotClient botClient,
    Update update,
    CancellationToken cancellationToken)
{
    telegramUserIdStore.Id = null;
    try
    {
        switch (update.Type)
        {
            // A message was received
            case UpdateType.Message:
                await HandleMessage(update.Message!, cancellationToken);
                break;

            // A button was pressed
            case UpdateType.CallbackQuery:
                await HandleButton(update.CallbackQuery!, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

static Task HandleErrorAsync(
    ITelegramBotClient botClient,
    Exception exception,
    CancellationToken cancellationToken)
{
    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    return Task.CompletedTask;
}

async Task HandleMessage(Message msg, CancellationToken cancellationToken)
{
    var user = msg.From;
    var messageText = msg.Text ?? string.Empty;

    if (user is null)
    {
        return;
    }

    var userId = user.Id;

    if (messageText.StartsWith("/"))
    {
        await HandleCommand(user, messageText, cancellationToken);
    }
    else if (messageText.Length > 0)
    {
        telegramUserIdStore.Id = userId;
        var currentButtonsGroup = await telegramUserCurrentButtonGroupService.GetGroupByUserId(userId, cancellationToken);
        var currentOperation = currentButtonsGroup.Operation;
        switch (currentOperation)
        {
            case ButtonsGroupOperation.CreateSpending:
                var spendingParseResult = spendingMessageParser.TryParse(messageText, out var parsingResult);
                if (!spendingParseResult)
                {
                    await bot.SendTextMessageAsync(user.Id, parsingResult.ErrorMessage, entities: msg.Entities, cancellationToken: cancellationToken);
                    return;
                }

                // await bot.SendTextMessageAsync(user.Id, "Обработка...", cancellationToken: cancellationToken);
                var request = new CreateSpendingRequest
                {
                    Amount = parsingResult.Amount,
                    TelegramUserId = userId,
                    Date = parsingResult.Date ?? DateTimeOffset.UtcNow,
                    Description = parsingResult.Description
                };
                await gatewayService.CreateSpendingAsync(request, cancellationToken);

                var nextGroup = currentButtonsGroup.Next;
                await bot.SendTextMessageAsync(
                    userId,
                    nextGroup.DefaultText,
                    parseMode: ParseMode.Html,
                    replyMarkup: nextGroup.Markup,
                    cancellationToken: cancellationToken
                );
                
                await telegramUserCurrentButtonGroupService.Update(userId, nextGroup, cancellationToken);

                break;
            case ButtonsGroupOperation.None:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

async Task HandleCommand(User user, string command, CancellationToken cancellationToken)
{
    switch (command)
    {
        case "/menu":
            await gatewayService.ProcessStartButton(bot, user, cancellationToken);
            break;
    }
}

async Task HandleButton(CallbackQuery query, CancellationToken cancellationToken)
{
    var userId = query.From.Id;
    telegramUserIdStore.Id = userId;

    var buttonClickHandleData = ButtonClickHandleData.Deserialize(query.Data!);
    var nextGroupId = buttonClickHandleData.NextGroupId;
    var nextGroup = await buttonsGroupManager.GetById(nextGroupId);

    var currentButtonsGroup = await telegramUserCurrentButtonGroupService.GetGroupByUserId(userId, cancellationToken);
    
    await bot.AnswerCallbackQueryAsync(query.Id, cancellationToken: cancellationToken);

    if (currentButtonsGroup.Id != buttonClickHandleData.CurrentGroupId)
    {
        // Пользователь нажал на кнопку группы, на которой он сейчас не находится. Игнорируем.
        return;
    }
    
    var currentGroup = await buttonsGroupManager.GetById(buttonClickHandleData.CurrentGroupId);
    if (currentGroup.Operation == ButtonsGroupOperation.ChangeCurrency && !string.IsNullOrEmpty(buttonClickHandleData.Id))
    {
        var selectedCurrencyCode = buttonClickHandleData.Id;
        await gatewayService.ChangeUserCurrency(userId, selectedCurrencyCode, cancellationToken);
        await bot.SendTextMessageAsync(
            userId,
            $"Валюта {selectedCurrencyCode} выбрана в качестве валюты по-умолчанию",
            cancellationToken: cancellationToken
        );
    }

    if (buttonClickHandleData.ShouldReplacePrevious)
    {
        await bot.EditMessageTextAsync(
            query.Message!.Chat.Id,
            query.Message.MessageId,
            nextGroup.DefaultText,
            ParseMode.Html,
            replyMarkup: nextGroup.Markup,
            cancellationToken: cancellationToken
        );
    }
    else
    {
        await bot.SendTextMessageAsync(
            userId,
            nextGroup.DefaultText,
            parseMode: ParseMode.Html,
            replyMarkup: nextGroup.Markup,
            cancellationToken: cancellationToken
        );
    }
  
    await telegramUserCurrentButtonGroupService.Update(userId, nextGroup, cancellationToken);
}

IServiceProvider InitializeDependencies()
{
    var configuration = AppConfigurationBuilder.Build();

    var builder = new HostBuilder()
        .ConfigureServices((_, services) =>
        {
            var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
            var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
            services.AddLogging(configure => configure.AddConsole())
                .AddInfrastructure(configuration)
                .AddDispatcher(assembliesForScan)
                .AddGenericSubDomain(configuration)
                .AddMemoryCache()
                .AddServices()
                .AddTelegramBotWrappingServices();
        }).UseConsoleLifetime();
    return builder.Build().Services;
}