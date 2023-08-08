using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpendingTracker.Application;
using SpendingTracker.Application.Spending.CreateSpending;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot;
using SpendingTracker.TelegramBot.SpendingParsing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient("6133107700:AAFPgfpteJtzLfauIHkmobDp8JbNNxrIwm0");

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
    }).UseConsoleLifetime();
var serviceProvider = builder.Build().Services;
IMediator mediator = new Mediator(serviceProvider);

var currencyUserRepository = serviceProvider.GetService<IUserCurrencyRepository>();

var startButtonsGroup = new ButtonGroup("Выберите действие");
var level2ButtonsGroup = new ButtonGroup(
    "Введите трату в формате сумма/дата/описание (каждое значение на новой строке)",
    UserOperationEnum.CreateSpending);

startButtonsGroup.AddButtonsLayer(
    new Button("Добавить трату", level2ButtonsGroup, startButtonsGroup),
    new Button("Перейти на сайт", "https://www.google.com", startButtonsGroup));

level2ButtonsGroup.AddButtonsLayer(new Button("Назад", startButtonsGroup, level2ButtonsGroup));
var buttonsGroups = new []
{
    startButtonsGroup,
    level2ButtonsGroup,
};

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

                    var userCurrency = await currencyUserRepository!.Get(new UserKey(userId));

                    var command = new CreateSpendingCommand
                    {
                        Amount = parsingResult.Amount,
                        Currency = userCurrency,
                        Date = parsingResult.Date ?? DateTimeOffset.Now,
                        Description = parsingResult.Description
                    };
                    await mediator.SendCommandAsync(command, cancellationToken);
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
    var targetButtonsGroup = buttonsGroups.First(g => g.Id.ToString() == query.Data);

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
