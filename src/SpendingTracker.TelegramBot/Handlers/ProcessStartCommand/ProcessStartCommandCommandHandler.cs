using MediatR;
using SpendingTracker.Application.Handlers.User.CreateUserByTelegramId.Contracts;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Handlers.ProcessStartCommand.Contracts;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace SpendingTracker.TelegramBot.Handlers.ProcessStartCommand;

internal sealed class ProcessStartCommandCommandHandler : CommandHandler<ProcessStartCommandCommand>
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly IUserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly ITelegramUserCurrentButtonGroupService _telegramUserCurrentButtonGroupService;

    public ProcessStartCommandCommandHandler(
        TelegramBotClient telegramBotClient,
        IUserRepository userRepository,
        IMediator mediator,
        ITelegramUserCurrentButtonGroupService telegramUserCurrentButtonGroupService)
    {
        _telegramBotClient = telegramBotClient;
        _userRepository = userRepository;
        _mediator = mediator;
        _telegramUserCurrentButtonGroupService = telegramUserCurrentButtonGroupService;
    }

    public override async Task Handle(ProcessStartCommandCommand commandCommand, CancellationToken cancellationToken)
    {
        var telegramUser = commandCommand.TelegramUser;
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
        
        var text = $"Текущая валюта - {user.Currency.Title}{user.Currency.CountryIcon} ({user.Currency.Code})";
        await _telegramBotClient.SendTextMessageAsync(
            telegramUser.Id,
            text,
            parseMode: ParseMode.Html,
            replyMarkup: ButtonsGroupStore.StartGroup.Markup,
            cancellationToken: cancellationToken
        );

        await _telegramUserCurrentButtonGroupService.Update(telegramUser.Id, ButtonsGroupStore.StartGroup, cancellationToken);
    }
}