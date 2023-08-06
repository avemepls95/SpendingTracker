using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot;

public class ButtonGroup
{
    public string Text { get; }

    public int Id { get; }

    private readonly List<Button> _buttons = new();
    public InlineKeyboardMarkup MarkUp
    {
        get => new (_buttons.Select(b => new [] { b.TelegramButton } ).ToArray());
    }

    public ButtonGroup(int id, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentNullException(nameof(text));
        }

        Id = id;
        Text = text;
    }

    public ButtonGroup AddButtons(params Button[] button)
    {
        _buttons.AddRange(button);

        return this;
    }
}