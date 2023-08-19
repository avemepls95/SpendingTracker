using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace SpendingTracker.TelegramBot.Buttons;

[Serializable]
public record ButtonClickHandleData
{
    [JsonProperty("C")]
    public int CurrentGroupId { get; init; }

    [JsonProperty("N")]
    public int NextGroupId { get; init; }

    [JsonProperty("S")]
    public bool ShouldReplacePrevious { get; init; }

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