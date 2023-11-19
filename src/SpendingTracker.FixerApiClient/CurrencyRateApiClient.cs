using FixerApiClient.Converters;
using FixerApiClient.FixerApiModel.GetLastRates;
using FixerApiClient.FixerApiModel.GetRatesByDates;
using Newtonsoft.Json;
using RestSharp;
using SpendingTracker.ApiClient;

namespace FixerApiClient;

internal sealed class CurrencyRateApiClient : ICurrencyRateApiClient
{
    public async Task<ExternalCallResult<CurrencyRateFromApi[]>> GetToday(
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

    public async Task<ExternalCallResult<CurrencyRateByDayFromApi[]>> GetByDates(
        string baseCode,
        string[] codes,
        DateOnly[] dates,
        CancellationToken cancellationToken)
    {
        var dateFrom = dates.Min();
        var dateTo = dates.Max();
        
        var client = new RestClient("https://api.apilayer.com/");

        var request = new RestRequest("fixer/timeseries");
        request.AddHeader("apikey", "9zqhyaS0QlL75JzGEno9W2eKGrG2fuOH");
        request.AddParameter("symbols", string.Join("%2C", codes));
        request.AddParameter("base", baseCode);
        request.AddParameter("start_date", dateFrom.ToString("yyyy-MM-dd"));
        request.AddParameter("end_date", dateTo.ToString("yyyy-MM-dd"));
        request.Method = Method.Get;

        try
        {
            var response = await client.ExecuteAsync(request, cancellationToken);
            if (!response.IsSuccessful)
            {
                return ExternalCallResult<CurrencyRateByDayFromApi[]>.CreateError(response.ErrorMessage!);
            }
            
            if (response.Content is null)
            {
                return ExternalCallResult<CurrencyRateByDayFromApi[]>.CreateError("Не удалось получить данные из источника курсов валют");
            }
            
            var deserializedResponse = JsonConvert.DeserializeObject<GetRatesByDatesResponse>(response.Content);
            if (deserializedResponse is null)
            {
                return ExternalCallResult<CurrencyRateByDayFromApi[]>.CreateError("Не удалось десериализовать данные из источника курсов валют");
            }
                
            var result = ApiModelConverter.ConvertGetRatesByDaysModels(deserializedResponse);
            return ExternalCallResult<CurrencyRateByDayFromApi[]>.CreateSuccess(result);

        }
        catch (Exception e)
        {
            return ExternalCallResult<CurrencyRateByDayFromApi[]>.CreateError(e.ToString());
        }
    }
}