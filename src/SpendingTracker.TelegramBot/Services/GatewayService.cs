using MediatR;
using SpendingTracker.Application.Spending.CreateSpending;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Services.Model;

namespace SpendingTracker.TelegramBot.Services;

public class GatewayService
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public GatewayService(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    public async Task CreateSpendingAsync(CreateSpendingRequest request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByTelegramId(request.TelegramUserId, cancellationToken);
        var command = new CreateSpendingCommand
        {
            Amount = request.Amount,
            UserKey = user.Id,
            Date = request.Date,
            Description = request.Description
        };

        await _mediator.Publish(command, cancellationToken);
    }
}