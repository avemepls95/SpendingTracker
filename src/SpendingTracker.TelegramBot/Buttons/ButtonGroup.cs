using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Buttons;

public class ButtonGroup
{
    public string Text { get; }
    public ButtonsGroupOperation Operation { get; } = ButtonsGroupOperation.None;
    public int Id { get; }
    public ButtonGroup Next { get; protected set; }

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
    
    public ButtonGroup(int id, ButtonsGroupOperation operation, string? text = null, ButtonGroup? next = null)
    {
        if (operation is ButtonsGroupOperation.None)
        {
            return;
        }

        Id = id;
        Operation = operation;
        Next = next;

        Text = string.IsNullOrWhiteSpace(text)
            ? ButtonsGroupTextProvider.GetText(operation)
            : text;
    }
    
    public ButtonGroup AddButtonsLayer(params Button[] buttons)
    {
        _buttons.Add(buttons);

        return this;
    }
}