using System.Reflection;
using System.Text;
using MediatR;
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
using SpendingTracker.TelegramBot.Handlers.CreateIncome.Contracts;
using SpendingTracker.TelegramBot.Handlers.CreateSpending.Contracts;
using SpendingTracker.TelegramBot.Handlers.ProcessButtonClick.Contracts;
using SpendingTracker.TelegramBot.Handlers.ProcessStart.Contracts;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;
using SpendingTracker.TelegramBot.TextMessageParsing;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var configuration = AppConfigurationBuilder.Build();

var connectionStrings = ConfigurationReader.ReadConnectionStrings(configuration);
var telegramOptions = ConfigurationReader.ReadTelegramOptions(configuration);

Console.OutputEncoding = Encoding.UTF8;
var bot = new TelegramBotClient(telegramOptions.Token);

var serviceProvider = InitializeDependencies();
var telegramUserIdStore = serviceProvider.GetService<ITelegramUserIdStore>()!;
var mediator = serviceProvider.GetService<IMediator>()!;
var telegramUserCurrentButtonGroupService = serviceProvider.GetService<ITelegramUserCurrentButtonGroupService>()!;

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
                await mediator.SendCommandAsync(new ProcessButtonClickCommand
                {
                    CallbackQuery = update.CallbackQuery!
                }, cancellationToken);
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
                await mediator.SendCommandAsync(new CreateSpendingCommand
                {
                    TelegramUserId = userId,
                    Message = msg
                }, cancellationToken);
                break;
            case ButtonsGroupType.CreateIncome:
            case ButtonsGroupType.CreateAnotherIncome:
                await mediator.SendCommandAsync(new CreateIncomeCommand
                {
                    TelegramUserId = userId,
                    Message = msg
                }, cancellationToken);
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

async Task HandleCommand(User user, string commandAsString, CancellationToken cancellationToken)
{
    switch (commandAsString)
    {
        case "/menu":
        case "/start":
            var command = new ProcessStartCommandCommand { TelegramUser = user };
            await mediator.SendCommandAsync(command, cancellationToken);
            break;
    }
}

IServiceProvider InitializeDependencies()
{
    var builder = new HostBuilder()
        .ConfigureServices((_, services) =>
        {
            var assemblyNamesForScan = new [] { "SpendingTracker.Application", "SpendingTracker.TelegramBot" };
            var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
            services.AddLogging(configure => configure.AddConsole())
                .AddInfrastructure(connectionStrings)
                .AddDispatcher(assembliesForScan)
                .AddFluentValidation(assembliesForScan)
                .AddGenericSubDomain(telegramOptions)
                .AddMemoryCache()
                .AddServices();

            services.AddSingleton<TelegramBotClient>(_ => bot);
            services.AddScoped<CreateAnotherSpendingGroupTransformer>();
            services.AddScoped<ChangeCurrencyGroupTransformer>();
            services.AddScoped<CreateIncomeGroupTransformer>();
            services.AddScoped<IButtonsGroupTransformerProvider, ButtonsGroupTransformerProvider>();
            services.AddScoped<ITextMessageParser, TextMessageParser>();
        }).UseConsoleLifetime();

    var host = builder.Build();
    
    var context = host.Services.GetRequiredService<MainDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
    
    return host.Services;
}