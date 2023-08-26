using System.Text.Json;
using Newtonsoft.Json;

namespace SpendingTracker.TelegramBot.Internal.Buttons;

[Serializable]
public record ButtonClickHandleData
{
    [JsonProperty("cg")]
    public int CurrentGroupId { get; init; }

    [JsonProperty("ng")]
    public int NextGroupId { get; init; }

    [JsonProperty("S")]
    public bool ShouldReplacePrevious { get; init; }
    
    [JsonProperty("i")]
    public string Id { get; set; }

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