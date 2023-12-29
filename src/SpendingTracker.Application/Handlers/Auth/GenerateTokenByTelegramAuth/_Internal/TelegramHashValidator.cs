using SpendingTracker.GenericSubDomain.User;
using Telegram.Bot.Extensions.LoginWidget;

namespace SpendingTracker.Application.Handlers.Auth.GenerateTokenByTelegramAuth._Internal;

internal class TelegramHashValidator : ITelegramHashValidator
{
    private readonly TelegramOptions _telegramOptions;

    public TelegramHashValidator(TelegramOptions telegramOptions)
    {
        _telegramOptions = telegramOptions;
    }

    public bool IsValid(
        string hash,
        string authDateAsString,
        string firstName,
        string? lastName,
        long userId,
        string? photoUrl,
        string? userName)
    {
        var data = new Dictionary<string, string>();
        
        if (string.IsNullOrWhiteSpace(hash))
        {
            throw new ArgumentNullException(nameof(hash));
        }

        if (string.IsNullOrWhiteSpace(authDateAsString))
        {
            throw new ArgumentNullException(nameof(hash));
        }
        
        data.Add("hash", hash);
        data.Add("auth_date", authDateAsString);
        data.Add("id", userId.ToString());

        if (!string.IsNullOrWhiteSpace(firstName)) { data.Add("first_name", firstName); }
        if (!string.IsNullOrWhiteSpace(lastName)) { data.Add("last_name", lastName); }
        if (!string.IsNullOrWhiteSpace(photoUrl)) { data.Add("photo_url", photoUrl); }
        if (!string.IsNullOrWhiteSpace(userName)) { data.Add("username", userName); }
  
        var loginWidget = new LoginWidget(_telegramOptions.Token);
        var authorizationResult = loginWidget.CheckAuthorization(data);
        return authorizationResult == Authorization.Valid;
    }
}