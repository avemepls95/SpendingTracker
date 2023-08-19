using MediatR;
using SpendingTracker.Application.Spending.CreateSpending;
using SpendingTracker.Application.User.CreateUserByTelegramId;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Buttons;
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

    public GatewayService(
        IMediator mediator,
        IUserRepository userRepository,
        ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService)
    {
        _mediator = mediator;
        _userRepository = userRepository;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
    }

    public async Task CreateSpendingAsync(CreateSpendingRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTelegramId(request.TelegramUserId, cancellationToken);
        var command = new CreateSpendingCommand
        {
            Amount = request.Amount,
            User = user,
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
        if (user == null)
        {
            await _mediator.SendCommandAsync(
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

        var startButtonsGroup = ButtonsGroupManager.GetInstance().StartButtonsGroup;
        var text = $"Текущая валюта - {user.Currency.Title} ({user.Currency.Code})";
        await telegramBotClient.SendTextMessageAsync(
            telegramUser.Id,
            text,
            parseMode: ParseMode.Html,
            replyMarkup: startButtonsGroup.Markup,
            cancellationToken: cancellationToken
        );

        await _telegramUserCurrentButtonGroupService.Update(telegramUser.Id, startButtonsGroup, cancellationToken);
    }
}