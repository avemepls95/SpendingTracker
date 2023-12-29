using Microsoft.Extensions.Configuration;
using SpendingTracker.GenericSubDomain.User;
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
    
    public static TelegramOptions ReadTelegramOptions(IConfiguration configuration)
    {
        var token = Environment.GetEnvironmentVariable("TELEGRAM-OPTIONS_TOKEN");

        if (!string.IsNullOrWhiteSpace(token))
        {
            return new TelegramOptions
            {
                Token = token
            };
        }

        var systemUserContextOptions = configuration.GetSection(nameof(TelegramOptions)).Get<TelegramOptions>();
        if (systemUserContextOptions is null)
        {
            throw new Exception("Empty TelegramOptions");
        }
            
        return systemUserContextOptions;
    }
}