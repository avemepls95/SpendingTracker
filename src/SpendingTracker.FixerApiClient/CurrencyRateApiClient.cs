using FixerApiClient.Converters;
using FixerApiClient.FixerApiModel.GetLastRates;
using Newtonsoft.Json;
using RestSharp;
using SpendingTracker.ApiClient;

namespace FixerApiClient;

internal sealed class CurrencyRateApiClient : ICurrencyRateApiClient
{
    public async Task<ExternalCallResult<CurrencyRateFromApi[]>> GetByCodes(
        string baseCode,
        string[] codes,
        CancellationToken cancellationToken)
    {
        var client = new RestClient("https://api.apilayer.com/");

        var request = new RestRequest("fixer/latest");
        request.AddHeader("apikey", "9zqhyaS0QlL75JzGEno9W2eKGrG2fuOH");
        request.AddParameter("symbols", string.Join("%2C", codes));
        request.AddParameter("base", baseCode);
        request.Method = Method.Get;

        try
        {
            var response = await client.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessful)
            {
                return ExternalCallResult<CurrencyRateFromApi[]>.CreateError(response.ErrorMessage!);
            }

            if (response.Content is null)
            {
                return ExternalCallResult<CurrencyRateFromApi[]>.CreateError("Не удалось получить данные из источника курсов валют");
            }
            
            var deserializedResponse = JsonConvert.DeserializeObject<GetLastRatesResponse>(response.Content);
            if (deserializedResponse is null)
            {
                return ExternalCallResult<CurrencyRateFromApi[]>.CreateError("Не удалось десериализовать данные из источника курсов валют");
            }
                
            var result = ApiModelConverter.ConvertGetLastRatesModels(deserializedResponse);
            return ExternalCallResult<CurrencyRateFromApi[]>.CreateSuccess(result);

        }
        catch (Exception e)
        {
            return ExternalCallResult<CurrencyRateFromApi[]>.CreateError(e.ToString());
        }
    }
}