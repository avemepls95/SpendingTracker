using SpendingTracker.BearerTokenAuth;
using SpendingTracker.GenericSubDomain.User;
using SpendingTracker.Infrastructure;
using SpendingTracker.WebApp.CustomFilters;

namespace SpendingTracker.WebApp;

public static class ConfigurationReader
{
    public static OAuthOptions ReadOAuthOptions(IConfiguration configuration)
    {
        var secretKey = Environment.GetEnvironmentVariable("OAUTH-OPTIONS_SECRET-KEY");
        var issuer = Environment.GetEnvironmentVariable("OAUTH-OPTIONS_SECRET-ISSUER");
        var accessTokenLifetimeInMinutesAsString = Environment.GetEnvironmentVariable("OAUTH-OPTIONS_ACCESS-TOKEN-LIFETIME-IN-MINUTES");

        if (string.IsNullOrWhiteSpace(secretKey)
            && string.IsNullOrWhiteSpace(issuer)
            && string.IsNullOrWhiteSpace(accessTokenLifetimeInMinutesAsString))
        {
            var oAuthOptions = configuration.GetSection(nameof(OAuthOptions)).Get<OAuthOptions>();
            if (oAuthOptions is null)
            {
                throw new Exception("Empty OAuthOptions");
            }

            return oAuthOptions;
        }

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new Exception("Empty OAUTH-OPTIONS_SECRET-KEY");
        }

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new Exception("Empty OAUTH-OPTIONS_SECRET-ISSUER");
        }
        
        if (string.IsNullOrWhiteSpace(accessTokenLifetimeInMinutesAsString)
            || !int.TryParse(accessTokenLifetimeInMinutesAsString, out var accessTokenLifetimeInMinutes))
        {
            throw new Exception("Empty or invalid OAUTH-OPTIONS_ACCESS-TOKEN-LIFETIME-IN-MINUTES");
        }

        return new OAuthOptions
        {
            SecretKey = secretKey,
            Issuer = issuer,
            AccessTokenLifetimeInMinutes = accessTokenLifetimeInMinutes
        };
    }

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
    
    public static CorsOptions ReadCorsOptions(IConfiguration configuration)
    {
        var originsAsString = Environment.GetEnvironmentVariable("CORS-OPTIONS_ORIGINS");

        CorsOptions? corsOptions;
        
        if (!string.IsNullOrWhiteSpace(originsAsString))
        {
            var origins = originsAsString.Split(";");
            if (origins.Length == 0)
            {
                corsOptions = configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();
                if (corsOptions is null)
                {
                    throw new Exception("Empty CorsOptions");
                }
            }

            return new CorsOptions
            {
                Origins = origins
            };
        }

        corsOptions = configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();
        if (corsOptions is null)
        {
            throw new Exception("Empty CorsOptions");
        }

        return corsOptions;
    }
}