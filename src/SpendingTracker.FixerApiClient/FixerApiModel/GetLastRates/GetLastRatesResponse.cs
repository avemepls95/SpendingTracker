using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FixerApiClient.FixerApiModel.GetLastRates;

internal class GetLastRatesResponse
{
    [JsonProperty(PropertyName = "base")]
    public string BaseCode { get; set; }

    [JsonProperty(PropertyName = "date")]
    public DateTime Date { get; set; }

    [JsonProperty(PropertyName = "rates")]
    public object Rates { get; set; }

    [JsonProperty(PropertyName = "success")]
    public bool IsSuccess { get; set; }
}
