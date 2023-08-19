using Telegram.Bot.Types.ReplyMarkups;

namespace SpendingTracker.TelegramBot.Buttons;

public class ButtonGroup
{
    public string DefaultText { get; }
    public ButtonsGroupOperation Operation { get; } = ButtonsGroupOperation.None;
    public int Id { get; }
    public ButtonGroup Next { get; protected init; }

    private readonly List<Button[]> _buttons = new();
    public InlineKeyboardMarkup Markup
    {
        get => new (_buttons
                .Select(buttons => buttons.Select(b => b.TelegramButton).ToArray())
                .ToArray());
    }

    public ButtonGroup(int id, string defaultText)
    {
        if (string.IsNullOrWhiteSpace(defaultText))
        {
            throw new ArgumentNullException(nameof(defaultText));
        }

        Id = id;
        DefaultText = defaultText;
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

        DefaultText = string.IsNullOrWhiteSpace(text)
            ? ButtonsGroupTextProvider.GetText(operation)
            : text;
    }
    
    public ButtonGroup AddButtonsLayer(params Button[] buttons)
    {
        _buttons.Add(buttons);

        return this;
    }
}