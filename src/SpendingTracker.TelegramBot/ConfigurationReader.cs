using Microsoft.Extensions.Configuration;
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
}