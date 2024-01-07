using SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.Infrastructure.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.Infrastructure.Abstractions.Repositories.Models;

namespace SpendingTracker.Application.Handlers.UserSettings.UpdateUserSettings;

internal sealed class UpdateUserSettingsCommandHandler : CommandHandler<UpdateUserSettingsCommand>
{
    private readonly IUserSettingValuesRepository _userSettingValuesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserSettingsCommandHandler(
        IUserSettingValuesRepository userSettingValuesRepository,
        IUnitOfWork unitOfWork)
    {
        _userSettingValuesRepository = userSettingValuesRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task Handle(UpdateUserSettingsCommand command, CancellationToken cancellationToken)
    {
        var updateValues = new[]
        {
            new CreateOrUpdateUserSettingValuesModel
            {
                UserId = command.UserId,
                Key = UserSettingEnum.ViewCurrencyId,
                NewValueAsString = command.ViewCurrencyId.ToString()
            }
        };

        await _userSettingValuesRepository.CreateOrUpdate(updateValues, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);
    }
}