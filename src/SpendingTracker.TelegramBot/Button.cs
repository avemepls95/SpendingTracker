using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot;

public class Button
{
    public string Title { get; }
    public InlineKeyboardButton TelegramButton { get; }
    public ButtonGroup NavigationGroup { get; }
    public ButtonGroup ParentGroup { get; }

    public Button(string title, string url, ButtonGroup parentGroup)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        Title = title;
        TelegramButton = new InlineKeyboardButton(title) { Url = url };
        ParentGroup = parentGroup;
    }

    public Button(string title, ButtonGroup navigationGroup, ButtonGroup parentGroup)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (navigationGroup == null)
        {
            throw new ArgumentNullException(nameof(navigationGroup));
        }

        NavigationGroup = navigationGroup;

        Title = title;
        TelegramButton = InlineKeyboardButton.WithCallbackData(title, navigationGroup.Id.ToString());
        ParentGroup = parentGroup;
    }
}