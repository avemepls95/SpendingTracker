using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Buttons;

public class Button
{
    public string Title { get; }
    public InlineKeyboardButton TelegramButton { get; }

    public Button(string title, string url, ButtonGroup group)
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
    }

    public Button(string title, ButtonGroup navigationGroupByClick, ButtonGroup group, bool shouldEditPreviousMessage = true)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (navigationGroupByClick == null)
        {
            throw new ArgumentNullException(nameof(navigationGroupByClick));
        }

        Title = title;

        var handleData = new ButtonClickHandleData
        {
            NextGroupId = navigationGroupByClick.Id,
            ShouldReplacePrevious = shouldEditPreviousMessage,
            CurrentGroupId = group.Id
        };
        TelegramButton = InlineKeyboardButton.WithCallbackData(title, handleData.Serialize());
    }
}
