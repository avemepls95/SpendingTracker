using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Buttons;

public class Button
{
    public string Title { get; }
    public InlineKeyboardButton TelegramButton { get; }
    public ButtonGroup NavigationGroupByClick { get; }
    // public ButtonGroup ParentGroup { get; }

    public Button(string title, string url
        // , ButtonGroup parentGroup
        )
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
        // ParentGroup = parentGroup;
    }

    public Button(
        string title,
        ButtonGroup navigationGroupByClick)
        // ButtonGroup parentGroup,)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (navigationGroupByClick == null)
        {
            throw new ArgumentNullException(nameof(navigationGroupByClick));
        }

        NavigationGroupByClick = navigationGroupByClick;

        Title = title;
        TelegramButton = InlineKeyboardButton.WithCallbackData(title, navigationGroupByClick.Id.ToString());
        // ParentGroup = parentGroup;
    }
}