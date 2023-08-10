using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpendingTracker.Application;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;
using SpendingTracker.TelegramBot;
using SpendingTracker.TelegramBot.Services;
using SpendingTracker.TelegramBot.Services.Model;
using SpendingTracker.TelegramBot.SpendingParsing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient("6133107700:AAFPgfpteJtzLfauIHkmobDp8JbNNxrIwm0");

var serviceProvider = InitializeDependencies();
var gatewayService = serviceProvider.GetService<GatewayService>()!;

var buttonsGroupManager = ButtonsGroupManager.Initialize();

var spendingMessageParser = new SpendingMessageParser();

var userOperationDict = new Dictionary<long, UserOperationEnum?>();
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }, // receive all update types
};
bot.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken
);
Console.ReadLine();

async Task HandleUpdateAsync(
    ITelegramBotClient botClient,
    Update update,
    CancellationToken cancellationToken)
{
    switch (update.Type)
    {
        // A message was received
        case UpdateType.Message:
            await HandleMessage(update.Message!);
            break;

        // A button was pressed
        case UpdateType.CallbackQuery:
            await HandleButton(update.CallbackQuery!);
            break;
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

async Task HandleMessage(Message msg)
{
    var user = msg.From;
    var messageText = msg.Text ?? string.Empty;

    if (user is null)
        return;

    var userId = user.Id;

    if (messageText.StartsWith("/"))
    {
        await HandleCommand(user.Id, messageText);
    }
    else if (messageText.Length > 0)
    {
        var rowByOperationExists = userOperationDict.TryGetValue(userId, out var userOperation);
        if (rowByOperationExists && userOperation.HasValue)
        {
            switch (userOperation)
            {
                case UserOperationEnum.CreateSpending:
                    var spendingParseResult = spendingMessageParser.TryParse(messageText, out var parsingResult);
                    if (!spendingParseResult)
                    {
                        await bot.SendTextMessageAsync(user.Id, parsingResult.ErrorMessage, entities: msg.Entities);
                        return;
                    }

                    var request = new CreateSpendingRequest
                    {
                        Amount = parsingResult.Amount,
                        TelegramUserId = userId,
                        Date = parsingResult.Date ?? DateTimeOffset.Now,
                        Description = parsingResult.Description
                    };
                    await gatewayService.CreateSpendingAsync(request, cancellationToken);
                    await bot.SendTextMessageAsync(user.Id, "Done", entities: msg.Entities);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

async Task HandleCommand(long userId, string command)
{
    switch (command)
    {
        case "/start":
            var startButtonsGroup = buttonsGroupManager.StartButtonsGroup;
            await bot.SendTextMessageAsync(
                userId,
                startButtonsGroup.Text,
                parseMode: ParseMode.Html,
                replyMarkup: startButtonsGroup.MarkUp
            );
            break;
    }

    await Task.CompletedTask;
}

async Task HandleButton(CallbackQuery query)
{
    var targetButtonsGroup = buttonsGroupManager.GetById(query.Data);

    await bot.AnswerCallbackQueryAsync(query.Id);

    userOperationDict.Add(query.From.Id, targetButtonsGroup.Operation);

    await bot.EditMessageTextAsync(
        query.Message!.Chat.Id,
        query.Message.MessageId,
        targetButtonsGroup.Text,
        ParseMode.Html,
        replyMarkup: targetButtonsGroup.MarkUp
    );
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
                .AddGenericSubDomain(configuration);

            services.AddScoped<GatewayService>();
        }).UseConsoleLifetime();
    return builder.Build().Services;
}