using System.Security.Cryptography;
using System.Text;
using System.Web;
using SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth.Contracts;
using SpendingTracker.GenericSubDomain.User;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;

internal class TelegramHashValidator : ITelegramHashValidator
{
    private readonly TelegramOptions _telegramOptions;
    private static readonly DateTime UnixStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private const long AllowedTimeOffsetInSeconds = 30;

    public TelegramHashValidator(TelegramOptions telegramOptions)
    {
        _telegramOptions = telegramOptions;
    }

    public bool IsValid(string checkString, TelegramAuthType authType)
    {
        // Parse string initData from telegram.
        var data = HttpUtility.ParseQueryString(checkString);

        // Put data in a alphabetically sorted dict.
        var dataDict = new SortedDictionary<string, string>(
            data.AllKeys.ToDictionary(x => x!, x => data[x]!),
            StringComparer.Ordinal);

        if (!dataDict.TryGetValue("auth_date", out var authDateTicksAsString)
            || !long.TryParse(authDateTicksAsString, out var authDateTicks)
            || !AuthDateIsValid(authDateTicks))
        {
            return false;
        }

        // Data-check-string is a chain of all received fields,
        // sorted alphabetically.
        // in the format key=<value>.
        // with a line feed character ('\n', 0x0A) used as separator.
        // e.g., 'auth_date=<auth_date>\nquery_id=<query_id>\nuser=<user>'
        var dataCheckString = string.Join(
            '\n',
            dataDict
                .Where(x => x.Key != "hash") // Hash should be removed.
                .Select(x => $"{x.Key}={x.Value}")); // like auth_date=<auth_date> ..

        byte[] secretKeyAsByteArray;
        if (authType == TelegramAuthType.WebApp)
        {
            const string constantKey = "WebAppData";
            // secretKey is the HMAC-SHA-256 signature of the bot's token
            // with the constant string WebAppData used as a key.
            secretKeyAsByteArray = HMACSHA256.HashData(
                Encoding.UTF8.GetBytes(constantKey), // WebAppData
                Encoding.UTF8.GetBytes(_telegramOptions.Token)); // Bot's token            
        }
        else
        {
            using var hasher = SHA256.Create();
            secretKeyAsByteArray = hasher.ComputeHash(Encoding.UTF8.GetBytes(_telegramOptions.Token));   
        }

        var generatedHash = HMACSHA256.HashData(
            secretKeyAsByteArray,
            Encoding.UTF8.GetBytes(dataCheckString)); // data_check_string

        // Convert received hash from telegram to a byte array.
        var actualHash = Convert.FromHexString(dataDict["hash"]);

        // Compare our hash with the one from telegram.
        return actualHash.SequenceEqual(generatedHash);
    }

    private static bool AuthDateIsValid(long authDateTicks)
    {
        var now = DateTime.UtcNow.Subtract(UnixStart).TotalSeconds;
        return Math.Abs(now - authDateTicks) <= AllowedTimeOffsetInSeconds;
    }
}