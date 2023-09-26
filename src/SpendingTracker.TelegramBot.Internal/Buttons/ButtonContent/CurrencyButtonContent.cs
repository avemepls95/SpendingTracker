namespace SpendingTracker.TelegramBot.Internal.Buttons.ButtonContent;

public class CurrencyButtonContent : ButtonContent
{
    public CurrencyButtonContent(string code, string countryIcon)
    {
        Code = code;
        CountryIcon = countryIcon;
    }

    public string Code { get; }
    public string CountryIcon { get; }
    
    public static CurrencyButtonContent Deserialize(string data)
    {
        var propertiesAsString = data.Split(",");
        var code = propertiesAsString[0];
        var countryIcon = propertiesAsString[1];

        var result = new CurrencyButtonContent(code, countryIcon);
        
        return result;
    }

    public override string Serialize()
    {
        return $"{Code},{CountryIcon}";
    }
}