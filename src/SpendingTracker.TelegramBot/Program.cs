using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
using SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;
using SpendingTracker.TelegramBot.Services;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.Model;
using SpendingTracker.TelegramBot.SpendingParsing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var telegramBotToken = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");
var bot = new TelegramBotClient(telegramBotToken);
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
            
            case UpdateType.MyChatMember:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    catch (Exception e)
    {
        var user = update.Type == UpdateType.Message
            ? update.Message!.From!
            : update.CallbackQuery!.From;
        var userId = user.Id;

        await bot.SendTextMessageAsync(
            userId,
            "Произошла непредвиденная ошибка",
            cancellationToken: cancellationToken);

        await bot.SendTextMessageAsync(
            375036212,
            $"Пользователь {user.LastName} {user.FirstName}{Environment.NewLine}{e}",
            cancellationToken: cancellationToken);    
        
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
        var currentOperation = currentButtonsGroup.Type;
        switch (currentOperation)
        {
            case ButtonsGroupType.CreateSpending:
            case ButtonsGroupType.CreateAnotherSpending:
                var spendingMessageParsingResult = spendingMessageParser.Parse(messageText);
                if (!spendingMessageParsingResult.IsSuccess)
                {
                    await bot.SendTextMessageAsync(
                        user.Id,
                        spendingMessageParsingResult.ErrorMessage,
                        entities: msg.Entities,
                        cancellationToken: cancellationToken);

                    return;
                }

                var request = new CreateSpendingRequest
                {
                    Amount = spendingMessageParsingResult.Amount,
                    TelegramUserId = userId,
                    Date = spendingMessageParsingResult.Date ?? DateTimeOffset.UtcNow.UtcDateTime,
                    Description = spendingMessageParsingResult.Description
                };
                await gatewayService.CreateSpendingAsync(request, cancellationToken);

                var nextGroup = currentButtonsGroup.Next;
                await bot.SendTextMessageAsync(
                    userId,
                    nextGroup.Text,
                    parseMode: ParseMode.Html,
                    replyMarkup: nextGroup.Markup,
                    cancellationToken: cancellationToken
                );
                
                await telegramUserCurrentButtonGroupService.Update(userId, nextGroup, cancellationToken);

                break;
            case ButtonsGroupType.None:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    else
    {
        await bot.SendTextMessageAsync(
            userId,
            "Отправлено пустое сообщение",
            cancellationToken: cancellationToken
        );
    }
}

async Task HandleCommand(User user, string command, CancellationToken cancellationToken)
{
    switch (command)
    {
        case "/menu":
        case "/start":
            await gatewayService.ProcessStartButton(bot, user, cancellationToken);
            break;
    }
}

async Task HandleButton(CallbackQuery query, CancellationToken cancellationToken)
{
    var userId = query.From.Id;
    telegramUserIdStore.Id = userId;

    var buttonClickHandleData = ButtonClickHandleData.Deserialize(query.Data!);

    var currentButtonsGroup = await telegramUserCurrentButtonGroupService.GetGroupByUserId(userId, cancellationToken);
    
    await bot.AnswerCallbackQueryAsync(query.Id, cancellationToken: cancellationToken);

    if (currentButtonsGroup.Id != buttonClickHandleData.CurrentGroupId)
    {
        // Пользователь нажал на кнопку группы, на которой он сейчас не находится. Игнорируем.
        return;
    }

    if (currentButtonsGroup.Type == ButtonsGroupType.ChangeCurrency
        && !string.IsNullOrWhiteSpace(buttonClickHandleData.Content))
    {
        var currencyButtonContent = CurrencyButtonContent.Deserialize(buttonClickHandleData.Content);
        var selectedCurrencyCode = currencyButtonContent.Code;
        var selectedCurrencyCountryCode = currencyButtonContent.CountryIcon;
        await gatewayService.ChangeUserCurrency(userId, selectedCurrencyCode, cancellationToken);
        await bot.DeleteMessageAsync(query.Message!.Chat.Id, query.Message.MessageId, cancellationToken: cancellationToken);
        await bot.SendTextMessageAsync(
            userId,
            $"Валюта {selectedCurrencyCountryCode}{selectedCurrencyCode} выбрана в качестве валюты по-умолчанию",
            cancellationToken: cancellationToken
        );
    }
    
    if (buttonClickHandleData.Operation == ButtonOperation.DeleteLastSpending)
    {
        await gatewayService.DeleteLastSpending(userId, cancellationToken);
        await bot.DeleteMessageAsync(query.Message!.Chat.Id, query.Message.MessageId, cancellationToken: cancellationToken);
        await bot.SendTextMessageAsync(userId, $"✅ Трата удалена", cancellationToken: cancellationToken);
    }

    var nextGroupId = buttonClickHandleData.NextGroupId;
    var nextGroup = await buttonsGroupManager.ConstructById(nextGroupId, currentButtonsGroup.Id);
    if (buttonClickHandleData.ShouldReplacePrevious)
    {
        await bot.EditMessageTextAsync(
            query.Message!.Chat.Id,
            query.Message.MessageId,
            nextGroup.Text,
            ParseMode.Html,
            replyMarkup: nextGroup.Markup,
            cancellationToken: cancellationToken
        );
    }
    else
    {
        await bot.SendTextMessageAsync(
            userId,
            nextGroup.Text,
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

    var connectionStrings = ConfigurationReader.ReadConnectionStrings(configuration);
    var systemUserContextOptions = ConfigurationReader.ReadSystemUserContextOptions(configuration);
    var telegramUserContextOptions = ConfigurationReader.ReadTelegramUserContextOptions(configuration);

    var builder = new HostBuilder()
        .ConfigureServices((_, services) =>
        {
            var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
            var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
            services.AddLogging(configure => configure.AddConsole())
                .AddInfrastructure(connectionStrings)
                .AddDispatcher(assembliesForScan)
                .AddFluentValidation(assembliesForScan)
                .AddGenericSubDomain(systemUserContextOptions, telegramUserContextOptions)
                .AddMemoryCache()
                .AddServices()
                .AddTelegramBotWrappingServices();
        }).UseConsoleLifetime();

    var host = builder.Build();
    
    var context = host.Services.GetRequiredService<MainDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
    
    return host.Services;
}