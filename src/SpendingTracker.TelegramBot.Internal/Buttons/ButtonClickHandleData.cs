using System.Text;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

public record ButtonClickHandleData
{
    public int CurrentGroupId { get; init; }
    public int NextGroupId { get; init; }
    public bool ShouldReplacePrevious { get; init; }
    public string? Content { get; set; }
    public ButtonOperation? Operation { get; set; }

    public string Serialize()
    {
        var shouldReplacePreviousAsString = ShouldReplacePrevious ? "1" : "0";
        var resultBuilder = new StringBuilder($"{CurrentGroupId};{NextGroupId};{shouldReplacePreviousAsString};{Content}");

        if (Operation.HasValue)
        {
            resultBuilder.Append($";{(int)Operation}");
        }

        return resultBuilder.ToString();
    }
    
    public static ButtonClickHandleData Deserialize(string data)
    {
        var propertiesAsString = data.Split(";");
        var currentGroupId = int.Parse(propertiesAsString[0]);
        var nextGroupId = int.Parse(propertiesAsString[1]);
        var shouldReplacePrevious = int.Parse(propertiesAsString[2]) == 1;
        var content = propertiesAsString[3];

        var result = new ButtonClickHandleData
        {
            CurrentGroupId = currentGroupId,
            NextGroupId = nextGroupId,
            ShouldReplacePrevious = shouldReplacePrevious,
            Content = content
        };

        if (propertiesAsString.Length > 4)
        {
            result.Operation = (ButtonOperation)int.Parse(propertiesAsString[4]);
        }
        
        return result;
    }
}