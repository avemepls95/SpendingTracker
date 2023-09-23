using System.Text.Json;
using Newtonsoft.Json;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

[Serializable]
public record ButtonClickHandleData
{
    [JsonProperty("1")]
    public int CurrentGroupId { get; init; }

    [JsonProperty("2")]
    public int NextGroupId { get; init; }

    [JsonProperty("3")]
    public bool ShouldReplacePrevious { get; init; }
    
    [JsonProperty("4")]
    public string? Content { get; set; }

    [JsonProperty("5")]
    public ButtonOperation? Operation { get; set; }

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }
    
    public static ButtonClickHandleData Deserialize(string data)
    {
        var result = JsonConvert.DeserializeObject<ButtonClickHandleData>(data);
        if (result == null)
        {
            throw new ArgumentException("Не удалось десериализовать объект");
        }
        
        return result;
    }
}