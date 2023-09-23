using Newtonsoft.Json;

namespace SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;

[Serializable]
public class CurrencyButtonContent : ButtonContent
{
    public CurrencyButtonContent(string code, string countryIcon)
    {
        Code = code;
        CountryIcon = countryIcon;
    }

    [JsonProperty("c")]
    public string Code { get; }

    [JsonProperty("i")]
    public string CountryIcon { get; }
    
    public static CurrencyButtonContent Deserialize(string data)
    {
        var result = JsonConvert.DeserializeObject<CurrencyButtonContent>(data);
        if (result == null)
        {
            throw new ArgumentException("Не удалось десериализовать объект");
        }
        
        return result;
    }
}