using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class Button
{
    public string Title { get; }
    public InlineKeyboardButton TelegramButton { get; }

    public Button(string title, string url)
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

    public Button(
        string title,
        ButtonGroup navigationGroupByClick,
        ButtonGroup group,
        bool shouldEditPreviousMessage = true,
        ButtonContent.ButtonContent? content = null,
        ButtonOperation? operation = null)
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

        var clickHandleData = new ButtonClickHandleData
        {
            NextGroupId = navigationGroupByClick.Id,
            ShouldReplacePrevious = shouldEditPreviousMessage,
            CurrentGroupId = group.Id,
        };

        if (content is not null)
        {
            clickHandleData.Content = content.Serialize();
        }

        if (operation is not null)
        {
            clickHandleData.Operation = operation;
        }
        
        TelegramButton = InlineKeyboardButton.WithCallbackData(title, clickHandleData.Serialize());
    }
}
