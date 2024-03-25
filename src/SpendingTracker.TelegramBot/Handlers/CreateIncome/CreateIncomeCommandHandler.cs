using MediatR;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Handlers.CreateIncome.Contracts;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;
using SpendingTracker.TelegramBot.TextMessageParsing;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SpendingTracker.TelegramBot.Handlers.CreateIncome;

internal sealed class CreateIncomeCommandHandler : CommandHandler<CreateIncomeCommand>
{
    private readonly ITextMessageParser _textMessageParser;
    private readonly TelegramBotClient _telegramBotClient;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly IButtonsGroupTransformerProvider _buttonsGroupTransformerProvider;
    private readonly ITelegramUserCurrentButtonGroupService _telegramUserCurrentButtonGroupService;
    private readonly IAccountRepository _accountRepository;

    public CreateIncomeCommandHandler(
        ITextMessageParser textMessageParser,
        TelegramBotClient telegramBotClient,
        IUserRepository userRepository,
        IMediator mediator,
        IButtonsGroupTransformerProvider buttonsGroupTransformerProvider,
        ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService,
        IAccountRepository accountRepository)
    {
        _textMessageParser = textMessageParser;
        _telegramBotClient = telegramBotClient;
        _userRepository = userRepository;
        _mediator = mediator;
        _buttonsGroupTransformerProvider = buttonsGroupTransformerProvider;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
        _accountRepository = accountRepository;
    }

    public override async Task Handle(CreateIncomeCommand command, CancellationToken cancellationToken)
    {
        var incomeMessageParsingResult = _textMessageParser.ParseIncome(command.Message.Text!);
        if (!incomeMessageParsingResult.IsSuccess)
        {
            await _telegramBotClient.SendTextMessageAsync(
                command.TelegramUserId,
                incomeMessageParsingResult.ErrorMessage,
                entities: command.Message.Entities,
                cancellationToken: cancellationToken);

            return;
        }

        var userId = await _userRepository.GetIdByTelegramId(command.TelegramUserId, cancellationToken);

        Guid? accountId = null;
        if (incomeMessageParsingResult.AccountIndex.HasValue)
        {
            var orderedUserAccounts = await _accountRepository.GetOrderedByDateUserAccounts(userId, cancellationToken);
            if (incomeMessageParsingResult.AccountIndex > orderedUserAccounts.Length)
            {
                await _telegramBotClient.SendTextMessageAsync(
                    command.TelegramUserId,
                    "Набор счетов изменился. Повторите действие",
                    entities: command.Message.Entities,
                    cancellationToken: cancellationToken);
                // throw new Exception($"Выбранный индекс счета {incomeMessageParsingResult.AccountIndex} пользователя {userId} больше кол-ва счетов {orderedUserAccounts.Length}");
            }

            accountId = orderedUserAccounts[incomeMessageParsingResult.AccountIndex.Value - 1].Id;
        }

        await _mediator.SendCommandAsync(new Application.Handlers.Income.Create.Contracts.CreateIncomeCommand
        {
            Amount = incomeMessageParsingResult.Amount,
            UserId = userId,
            Date = incomeMessageParsingResult.Date ?? DateTimeOffset.UtcNow.UtcDateTime,
            Description = incomeMessageParsingResult.Description,
            AccountId = accountId
        }, cancellationToken);

        var nextGroup = ButtonsGroupStore.CreateAnotherIncomeGroup;
        var groupTransformer = _buttonsGroupTransformerProvider.Get(nextGroup.Id);
        await groupTransformer.Transform(nextGroup, nextGroup.Id, cancellationToken);
        await _telegramBotClient.SendTextMessageAsync(
            command.TelegramUserId,
            nextGroup.Text,
            parseMode: ParseMode.Html,
            replyMarkup: nextGroup.Markup,
            cancellationToken: cancellationToken
        );

        await _telegramUserCurrentButtonGroupService.Update(command.TelegramUserId, nextGroup, cancellationToken);
    }
}