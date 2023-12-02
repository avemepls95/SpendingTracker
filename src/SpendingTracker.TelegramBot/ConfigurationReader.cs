using Microsoft.Extensions.Configuration;
using SpendingTracker.GenericSubDomain.User.Internal;
using SpendingTracker.Infrastructure;

namespace SpendingTracker.TelegramBot;

public static class ConfigurationReader
{
    public static ConnectionStrings ReadConnectionStrings(IConfiguration configuration)
    {
        var spendingDbConnectionString = Environment.GetEnvironmentVariable("CONNECTION-STRINGS_SPENDING-TRACKER-DB");

        if (!string.IsNullOrWhiteSpace(spendingDbConnectionString))
        {
            return new ConnectionStrings
            {
                SpendingTrackerDb = spendingDbConnectionString
            };
        }

        var connectionStringsConfig = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
        if (connectionStringsConfig is null)
        {
            throw new Exception("Empty ConnectionStrings");
        }

        return connectionStringsConfig;
    }
    
    public static SystemUserContextOptions ReadSystemUserContextOptions(IConfiguration configuration)
    {
        var cacheAbsoluteExpirationRelativeToNowAsString = Environment.GetEnvironmentVariable("SYSTEM-USER-CONTEXT-OPTIONS_CACHE-ABSOLUTE-EXPIRATION-RELATIVE-TO-NOW");

        if (!string.IsNullOrWhiteSpace(cacheAbsoluteExpirationRelativeToNowAsString)
            && TimeSpan.TryParse(cacheAbsoluteExpirationRelativeToNowAsString, out var cacheAbsoluteExpirationRelativeToNow))
        {
            return new SystemUserContextOptions
            {
                CacheAbsoluteExpirationRelativeToNow = cacheAbsoluteExpirationRelativeToNow
            };
        }

        var systemUserContextOptions = configuration.GetSection(nameof(SystemUserContextOptions)).Get<SystemUserContextOptions>();
        if (systemUserContextOptions is null)
        {
            throw new Exception("Empty SystemUserContextOptions");
        }
            
        return systemUserContextOptions;
    }
    
    public static TelegramUserContextOptions ReadTelegramUserContextOptions(IConfiguration configuration)
    {
        var cacheAbsoluteExpirationRelativeToNowAsString = Environment.GetEnvironmentVariable("TELEGRAM-USER-CONTEXT-OPTIONS_CACHE-ABSOLUTE-EXPIRATION-RELATIVE-TO-NOW");

        if (!string.IsNullOrWhiteSpace(cacheAbsoluteExpirationRelativeToNowAsString)
            && TimeSpan.TryParse(cacheAbsoluteExpirationRelativeToNowAsString, out var cacheAbsoluteExpirationRelativeToNow))
        {
            return new TelegramUserContextOptions
            {
                CacheAbsoluteExpirationRelativeToNow = cacheAbsoluteExpirationRelativeToNow
            };
        }

        var systemUserContextOptions = configuration.GetSection(nameof(TelegramUserContextOptions)).Get<TelegramUserContextOptions>();
        if (systemUserContextOptions is null)
        {
            throw new Exception("Empty TelegramUserContextOptions");
        }
            
        return systemUserContextOptions;
    }
}