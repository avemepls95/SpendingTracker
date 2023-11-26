using SpendingTracker.BearerTokenAuth;
using SpendingTracker.GenericSubDomain.User.Internal;
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