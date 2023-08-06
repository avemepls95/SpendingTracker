using System.Text;
using SpendingTracker.TelegramBot;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var bot = new TelegramBotClient("6133107700:AAFPgfpteJtzLfauIHkmobDp8JbNNxrIwm0");

var startButtonsGroup = new ButtonGroup(1, "Первый уровень");
var level2ButtonsGroup = new ButtonGroup(2, "Второй уровень");
var level3ButtonsGroup = new ButtonGroup(3, "Третий уровень");
startButtonsGroup.AddButtons(
    new Button("Перейти на 2 уровень", level2ButtonsGroup),
    new Button("Перейти на сайт", "https://www.google.com"));

level2ButtonsGroup.AddButtons(
    new Button("Перейти на 3 уровень", level3ButtonsGroup),
    new Button("Вернуться на 1 уровень", startButtonsGroup),
    new Button("Перейти на сайт", "https://www.google.com"));

level3ButtonsGroup.AddButtons(
    new Button("Вернуться на 1 уровень", startButtonsGroup),
    new Button("Перейти на сайт", "https://www.google.com"));

var buttonsGroups = new []
{
    startButtonsGroup,
    level2ButtonsGroup,
    level3ButtonsGroup
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

    // When we get a command, we react accordingly
    if (text.StartsWith("/"))
    {
        await HandleCommand(user.Id, text);
    }
    else if (text.Length > 0)
    {
        // To preserve the markdown, we attach entities (bold, italic..)
        await bot.SendTextMessageAsync(user.Id, text.ToUpper(), entities: msg.Entities);
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
