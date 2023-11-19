using Newtonsoft.Json;

namespace FixerApiClient.FixerApiModel.GetRatesByDates;

public class GetRatesByDatesResponse
{
    [JsonProperty(PropertyName = "base")]
    public string BaseCode { get; set; }

    [JsonProperty(PropertyName = "rates")]
    public object Rates { get; set; }

    [JsonProperty(PropertyName = "success")]
    public bool IsSuccess { get; set; }
}