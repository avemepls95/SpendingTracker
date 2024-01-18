using SpendingTracker.Application.Handlers.Auth.CreateAuthLog.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.Auth.CreateAuthLog;

internal sealed class CreateAuthLogCommandHandler : CommandHandler<CreateAuthLogCommand>
{
    private readonly IAuthLogRepository _authLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateAuthLogCommandHandler(IAuthLogRepository authLogRepository, IUnitOfWork unitOfWork)
    {
        _authLogRepository = authLogRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(CreateAuthLogCommand command, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        await _authLogRepository.Create(command.UserId, command.Source, command.AdditionalData, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}