using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SpendingTracker.Application;
using SpendingTracker.Application.Spending.CrateSpending;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;
using SpendingTracker.TelegramBot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient("6133107700:AAFPgfpteJtzLfauIHkmobDp8JbNNxrIwm0");

var configuration = AppConfigurationBuilder.Build();

//setup our DI
var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
var serviceProvider = new ServiceCollection()
    .AddDispatcher(assembliesForScan)
    .AddGenericSubDomain(configuration)
    .AddInfrastructure(configuration)
    .BuildServiceProvider();

IMediator mediator = new Mediator(serviceProvider);

var startButtonsGroup = new ButtonGroup("Выберите действие");
var level2ButtonsGroup = new ButtonGroup("Введите трату в формате сумма/дата/описание (каждое значение на новой строке)");

startButtonsGroup.AddButtonsLayer(
    new Button("Добавить трату", level2ButtonsGroup),
    new Button("Перейти на сайт", "https://www.google.com"));

level2ButtonsGroup.AddButtonsLayer(new Button("Назад", startButtonsGroup));

var buttonsGroups = new []
{
    startButtonsGroup,
    level2ButtonsGroup,
};

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
    var text = msg.Text ?? string.Empty;

    if (user is null)
        return;

    if (text.StartsWith("/"))
    {
        await HandleCommand(user.Id, text);
    }
    else if (text.Length > 0)
    {
        var command = new CreateSpendingCommand();
        await mediator.SendCommandAsync(command, cancellationToken);
        await bot.SendTextMessageAsync(user.Id, "Done", entities: msg.Entities);
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

    await bot.EditMessageTextAsync(
        query.Message!.Chat.Id,
        query.Message.MessageId,
        targetButtonsGroup.Text,
        ParseMode.Html,
        replyMarkup: targetButtonsGroup.MarkUp
    );
}
