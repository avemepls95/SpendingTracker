using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

public class ButtonGroup
{
    public string Text { get; private set; }
    public ButtonsGroupType Type { get; } = ButtonsGroupType.None;
    public int Id { get; }
    public ButtonGroup Next { get; protected init; }

    private readonly List<Button[]> _buttons = new();
    public InlineKeyboardMarkup Markup
    {
        get => new (_buttons
                .Select(buttons => buttons.Select(b => b.TelegramButton).ToArray())
                .ToArray());
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
    
    public ButtonGroup(int id, ButtonsGroupType type, string? text = null, ButtonGroup? next = null)
    {
        if (type is ButtonsGroupType.None)
        {
            return;
        }

        Id = id;
        Type = type;
        Next = next;

        Text = string.IsNullOrWhiteSpace(text)
            ? ButtonsGroupTextProvider.GetText(type)
            : text;
    }
    
    public ButtonGroup AddButtonsLayer(params Button[] buttons)
    {
        _buttons.Add(buttons);

        return this;
    }

    public ButtonGroup SetText(string text)
    {
        Text = text;

        return this;
    }

    public void ClearButtons()
    {
        _buttons.Clear();
    }
}