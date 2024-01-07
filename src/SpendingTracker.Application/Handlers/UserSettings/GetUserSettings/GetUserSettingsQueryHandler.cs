using SpendingTracker.Application.Handlers.UserSettings.GetUserSettings.Contracts;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher;
using SpendingTracker.Domain.UserSettings;
using SpendingTracker.GenericSubDomain.Validation;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.Application.Handlers.UserSettings.GetUserSettings;

internal sealed class GetUserSettingsQueryHandler : QueryHandler<GetUserSettingsQuery, GetUserSettingsQueryResponse>
{
    private readonly IUserSettingValuesRepository _userSettingValuesRepository;
    private readonly IUserSettingsRepository _userSettingsRepository;
    private readonly IUserRepository _userRepository;

    public GetUserSettingsQueryHandler(
        IUserSettingValuesRepository userSettingValuesRepository,
        IUserSettingsRepository userSettingsRepository,
        IUserRepository userRepository)
    {
        _userSettingValuesRepository = userSettingValuesRepository;
        _userSettingsRepository = userSettingsRepository;
        _userRepository = userRepository;
    }

    public override async Task<GetUserSettingsQueryResponse> HandleAsync(
        GetUserSettingsQuery query,
        CancellationToken cancellationToken)
    {
        var isUserExists = await _userRepository.IsExistsById(query.UserId, cancellationToken);
        if (!isUserExists)
        {
            throw new SpendingTrackerValidationException(ValidationErrorCodeEnum.KeyNotFound);
        }
        
        var allSettings = await _userSettingsRepository.GetAll(cancellationToken);
        var values = await _userSettingValuesRepository.GetValues(query.UserId, cancellationToken);

        var result = new GetUserSettingsQueryResponse
        {
            ViewCurrencyId = GetViewCurrencyId(allSettings, values)
        };

        return result;
    }

    private static Guid GetViewCurrencyId(UserSetting[] allSettings, UserSettingValue[] values)
    {
        var viewCurrencyIdSetting = values.FirstOrDefault(v => v.Setting.Key == UserSettingEnum.ViewCurrencyId);
        if (viewCurrencyIdSetting is not null)
        {
            return Guid.Parse(viewCurrencyIdSetting.ValueAsString);
        }

        var defaultValueAsString = allSettings
            .First(s => s.Key == UserSettingEnum.ViewCurrencyId)
            .DefaultValueAsString;
            
        return Guid.Parse(defaultValueAsString);
    }
}