using MediatR;
using SpendingTracker.Application.Handlers.Spending.CreateSpending.Contracts;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.Application.Handlers.UserCurrency.ChangeUserCurrency.Contracts;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Abstractions;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;
using SpendingTracker.TelegramBot.Services.Model;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using User = Telegram.Bot.Types.User;

namespace SpendingTracker.TelegramBot.Services;

public class GatewayService
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramUserCurrentButtonGroupService _telegramUserCurrentButtonGroupService;
    private readonly IButtonsGroupManager _buttonsGroupManager;

    public GatewayService(
        IMediator mediator,
        IUserRepository userRepository,
        ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService,
        IButtonsGroupManager buttonsGroupManager)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
        _buttonsGroupManager = buttonsGroupManager;
    }

    public async Task CreateSpendingAsync(CreateSpendingRequest request, CancellationToken cancellationToken)
    {
        var userId = await _userRepository.GetIdByTelegramId(request.TelegramUserId, cancellationToken);
        var command = new CreateSpendingCommand
        {
            Amount = request.Amount,
            UserId = userId,
            Date = request.Date,
            Description = request.Description,
            ActionSource = ActionSource.Telegram
        };

        await _mediator.SendCommandAsync(command, cancellationToken);
    }

    public async Task ProcessStartButton(
        TelegramBotClient telegramBotClient,
        User telegramUser,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByTelegramId(telegramUser.Id, cancellationToken);
        if (user is null)
        {
            await _mediator.SendCommandAsync<CreateUserByTelegramCommand, UserKey>(
                new CreateUserByTelegramCommand
                {
                    TelegramUserId = telegramUser.Id,
                    FirstName = telegramUser.FirstName,
                    LastName = telegramUser.LastName,
                    UserName = telegramUser.Username,
                },
                cancellationToken);

            user = await _userRepository.GetByTelegramId(telegramUser.Id, cancellationToken);
        }

        var startButtonsGroup = _buttonsGroupManager.StartGroup;
        var text = $"Текущая валюта - {user.Currency.Title}{user.Currency.CountryIcon} ({user.Currency.Code})";
        await telegramBotClient.SendTextMessageAsync(
            telegramUser.Id,
            text,
            parseMode: ParseMode.Html,
            replyMarkup: startButtonsGroup.Markup,
            cancellationToken: cancellationToken
        );

        await _telegramUserCurrentButtonGroupService.Update(telegramUser.Id, startButtonsGroup, cancellationToken);
    }
    
    public async Task ChangeUserCurrency(long telegramUserId, string currencyCode,CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTelegramId(telegramUserId, cancellationToken);
        var command = new ChangeUserCurrencyCommand
        {
            UserId = user.Id,
            CurrenctCode = currencyCode
        };

        await _mediator.SendCommandAsync(command, cancellationToken);
    }
}