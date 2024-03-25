using MediatR;
using SpendingTracker.Application.Handlers.Income.DeleteLast.Contracts;
using SpendingTracker.Application.Handlers.Spending.DeleteLastSpending.Contracts;
using SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Handlers.ProcessButtonClick.Contracts;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SpendingTracker.TelegramBot.Handlers.ProcessButtonClick;

internal sealed class ProcessButtonClickCommandHandler : CommandHandler<ProcessButtonClickCommand>
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly Services.Abstractions.ITelegramUserCurrentButtonGroupService _telegramUserCurrentButtonGroupService;
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramUserIdStore _telegramUserIdStore;
    private readonly IButtonsGroupTransformerProvider _buttonsGroupTransformerProvider;

    public ProcessButtonClickCommandHandler(
        TelegramBotClient telegramBotClient,
        Services.Abstractions.ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService,
        IMediator mediator,
        IUserRepository userRepository, 
        ITelegramUserIdStore telegramUserIdStore,
        IButtonsGroupTransformerProvider buttonsGroupTransformerProvider)
    {
        _telegramBotClient = telegramBotClient;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
        _mediator = mediator;
        _userRepository = userRepository;
        _telegramUserIdStore = telegramUserIdStore;
        _buttonsGroupTransformerProvider = buttonsGroupTransformerProvider;
    }

    public override async Task Handle(ProcessButtonClickCommand command, CancellationToken cancellationToken)
    {
        var telegramUserId = command.CallbackQuery.From.Id;
        _telegramUserIdStore.Id = telegramUserId;

        var callbackQuery = command.CallbackQuery;
        var buttonClickHandleData = ButtonClickHandleData.Deserialize(callbackQuery.Data!);

        var currentButtonsGroup = await _telegramUserCurrentButtonGroupService.GetGroupByUserId(
            telegramUserId,
            cancellationToken);

        await _telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id, cancellationToken: cancellationToken);

        if (currentButtonsGroup.Id != buttonClickHandleData.OwnerGroupId)
        {
            // Пользователь нажал на кнопку группы, на которой он сейчас не находится. Игнорируем.
            return;
        }

        if (currentButtonsGroup.Type == ButtonsGroupType.ChangeCurrency
            && !string.IsNullOrWhiteSpace(buttonClickHandleData.Content))
        {
            await ProcessSelectCurrencyButtonClick(
                buttonClickHandleData.Content,
                telegramUserId,
                callbackQuery.Message!,
                cancellationToken);
        }

        if (buttonClickHandleData.Operation == ButtonOperation.DeleteLastSpending)
        {
            await ProcessDeleteLastSpendingButtonClick(telegramUserId, callbackQuery.Message!, cancellationToken);
        }
        
        if (buttonClickHandleData.Operation == ButtonOperation.DeleteLastIncome)
        {
            await ProcessDeleteLastIncomeButtonClick(telegramUserId, callbackQuery.Message!, cancellationToken);
        }

        var nextGroupId = buttonClickHandleData.NextGroupId;
        var nextGroup = ButtonsGroupStore.GetById(nextGroupId);
        var groupTransformer = _buttonsGroupTransformerProvider.Get(nextGroupId);
        await groupTransformer.Transform(nextGroup, currentButtonsGroup.Id, cancellationToken);
        if (buttonClickHandleData.ShouldReplacePrevious)
        {
            await _telegramBotClient.EditMessageTextAsync(
                callbackQuery.Message!.Chat.Id,
                callbackQuery.Message.MessageId,
                nextGroup.Text,
                ParseMode.Html,
                replyMarkup: nextGroup.Markup,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await _telegramBotClient.SendTextMessageAsync(
                telegramUserId,
                nextGroup.Text,
                parseMode: ParseMode.Html,
                replyMarkup: nextGroup.Markup,
                cancellationToken: cancellationToken
            );
        }

        await _telegramUserCurrentButtonGroupService.Update(telegramUserId, nextGroup, cancellationToken);
    }

    private async Task ProcessSelectCurrencyButtonClick(
        string content,
        long telegramUserId,
        Message telegramMessage,
        CancellationToken cancellationToken)
    {
        var currencyButtonContent = CurrencyButtonContent.Deserialize(content);
        var selectedCurrencyCode = currencyButtonContent.Code;
        var selectedCurrencyCountryCode = currencyButtonContent.CountryIcon;

        var userId = await _userRepository.GetIdByTelegramId(telegramUserId, cancellationToken);
        await _mediator.SendCommandAsync(new ChangeUserCurrencyCommand
        {
            UserId = userId,
            CurrencyCode = selectedCurrencyCode
        }, cancellationToken);

        await _telegramBotClient.DeleteMessageAsync(
            telegramMessage!.Chat.Id,
            telegramMessage.MessageId,
            cancellationToken: cancellationToken);

        await _telegramBotClient.SendTextMessageAsync(
            telegramUserId,
            $"Валюта {selectedCurrencyCountryCode}{selectedCurrencyCode} выбрана в качестве валюты по-умолчанию",
            cancellationToken: cancellationToken
        );
    }

    private async Task ProcessDeleteLastSpendingButtonClick(
        long telegramUserId,
        Message telegramMessage,
        CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetIdByTelegramId(telegramUserId, cancellationToken);

        await _mediator.SendCommandAsync(new DeleteLastSpendingCommand
        {
            UserId = userId,
            ActionSource = ActionSource.Telegram
        }, cancellationToken);

        await _telegramBotClient.DeleteMessageAsync(
            telegramMessage!.Chat.Id,
            telegramMessage.MessageId,
            cancellationToken: cancellationToken);

        await _telegramBotClient.SendTextMessageAsync(
            telegramUserId,
            $"✅ Трата удалена",
            cancellationToken: cancellationToken);
    }
    
    private async Task ProcessDeleteLastIncomeButtonClick(
        long telegramUserId,
        Message telegramMessage,
        CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetIdByTelegramId(telegramUserId, cancellationToken);

        await _mediator.SendCommandAsync(new DeleteLastIncomeCommand
        {
            UserId = userId,
            ActionSource = ActionSource.Telegram
        }, cancellationToken);

        await _telegramBotClient.DeleteMessageAsync(
            telegramMessage!.Chat.Id,
            telegramMessage.MessageId,
            cancellationToken: cancellationToken);

        await _telegramBotClient.SendTextMessageAsync(
            telegramUserId,
            $"✅ Доход удален",
            cancellationToken: cancellationToken);
    }
}