using Newtonsoft.Json;

namespace SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;

public abstract class ButtonContent
{
    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
}