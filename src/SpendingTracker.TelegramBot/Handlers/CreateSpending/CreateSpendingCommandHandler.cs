using MediatR;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Handlers.CreateSpending.Contracts;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;
using SpendingTracker.TelegramBot.TextMessageParsing;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SpendingTracker.TelegramBot.Handlers.CreateSpending;

public class CreateSpendingCommandHandler : CommandHandler<CreateSpendingCommand> 
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly ITelegramUserCurrentButtonGroupService _telegramUserCurrentButtonGroupService;
    private readonly ITextMessageParser _textMessageParser;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly IButtonsGroupTransformerProvider _buttonsGroupTransformerProvider;

    public CreateSpendingCommandHandler(
        TelegramBotClient telegramBotClient,
        ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService,
        ITextMessageParser textMessageParser,
        IUserRepository userRepository,
        IMediator mediator,
        IButtonsGroupTransformerProvider buttonsGroupTransformerProvider)
    {
        _telegramBotClient = telegramBotClient;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
        _textMessageParser = textMessageParser;
        _userRepository = userRepository;
        _mediator = mediator;
        _buttonsGroupTransformerProvider = buttonsGroupTransformerProvider;
    }

    public override async Task Handle(CreateSpendingCommand command, CancellationToken cancellationToken)
    {
        var spendingMessageParsingResult = _textMessageParser.ParseSpending(command.Message.Text!);
        if (!spendingMessageParsingResult.IsSuccess)
        {
            await _telegramBotClient.SendTextMessageAsync(
                command.TelegramUserId,
                spendingMessageParsingResult.ErrorMessage,
                entities: command.Message.Entities,
                cancellationToken: cancellationToken);
        
            return;
        }

        var userId = await _userRepository.GetIdByTelegramId(command.TelegramUserId, cancellationToken);
        await _mediator.SendCommandAsync(new Application.Handlers.Spending.CreateSpending.Contracts.CreateSpendingCommand()
        {
            Amount = spendingMessageParsingResult.Amount,
            UserId = userId,
            Date = spendingMessageParsingResult.Date ?? DateTimeOffset.UtcNow.UtcDateTime,
            Description = spendingMessageParsingResult.Description,
            ActionSource = ActionSource.Telegram
        }, cancellationToken);

        var nextGroup = ButtonsGroupStore.CreateAnotherSpendingGroup;
        var groupTransformer = _buttonsGroupTransformerProvider.Get(nextGroup.Id);
        await groupTransformer.Transform(nextGroup, nextGroup.Id);
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