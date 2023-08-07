using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot;

public class ButtonGroup
{
    public string Text { get; }

    public Guid Id { get; } = Guid.NewGuid();

    private readonly List<Button[]> _buttons = new();
    public InlineKeyboardMarkup MarkUp
    {
        get => new (_buttons
                .Select(buttons => buttons.Select(b => b.TelegramButton).ToArray())
                .ToArray());
    }

    public ButtonGroup(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentNullException(nameof(text));
        }

        Text = text;
    }

    public ButtonGroup AddButtonsLayer(params Button[] buttons)
    {
        _buttons.Add(buttons);

        return this;
    }
}