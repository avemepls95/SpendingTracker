using SpendingTracker.CurrencyRate.Abstractions;
using SpendingTracker.CurrencyRate.Contracts;
using SpendingTracker.Infrastructure.Abstractions.Repositories;

namespace SpendingTracker.CurrencyRate;

// TODO: тесты
internal class CurrencyConverter : ICurrencyConverter
{
    private readonly ICurrencyRateByDayRepository _currencyRateByDayRepository;

    public CurrencyConverter(ICurrencyRateByDayRepository currencyRateByDayRepository)
    {
        _currencyRateByDayRepository = currencyRateByDayRepository;
    }

    public async Task<GetCoefficientResult[]> GetCoefficients(
        GetCoefficientRequest[] requests,
        CancellationToken cancellationToken)
    {
        if (requests.All(r => r.CurrencyIdFrom == r.CurrencyIdTo))
        {
            return requests.Select(r => new GetCoefficientResult
            {
                Date = r.Date,
                CurrencyFrom = r.CurrencyIdFrom,
                CurrencyTo = r.CurrencyIdTo,
                Coefficient = 1
            }).ToArray();
        }

        var days = requests
            .Select(r => r.Date)
            .Distinct()
            .ToArray();

        var ratesByDays = await _currencyRateByDayRepository.GetRatesByDays(days, cancellationToken);
        var result = new List<GetCoefficientResult>();
        foreach (var request in requests)
        {
            var coefficientResult = new GetCoefficientResult
            {
                Date = request.Date,
                CurrencyFrom = request.CurrencyIdFrom,
                CurrencyTo = request.CurrencyIdTo,
            };

            if (request.CurrencyIdFrom == request.CurrencyIdTo)
            {
                coefficientResult.Coefficient = 1;
                result.Add(coefficientResult);
                continue;
            }

            var ratesByDay = ratesByDays.Where(r => r.Date.Day == request.Date.Day).ToArray();
            // После реализации механизма восстановления курсов это будет не нужно
            if (ratesByDay.Length == 0)
            {
                ratesByDay = await _currencyRateByDayRepository.GetRatesByNearestDay(request.Date, cancellationToken);
            }

            if (request.CurrencyIdFrom == CurrencyOptions.BaseCurrencyId)
            {
                var rate = ratesByDay.FirstOrDefault(r => r.Target == request.CurrencyIdTo);
                // После реализации механизма прогрузки курсов для новой валюты это будет не нужно
                if (rate is null)
                {
                    throw new Exception($"Нет курсов по валюте {request.CurrencyIdTo}");
                }

                coefficientResult.Coefficient = rate.Coefficient;
                result.Add(coefficientResult);
                continue;
            }

            if (request.CurrencyIdTo == CurrencyOptions.BaseCurrencyId)
            {
                var rate = ratesByDay.FirstOrDefault(r => r.Target == request.CurrencyIdFrom);
                // После реализации механизма прогрузки курсов для новой валюты это будет не нужно
                if (rate is null)
                {
                    throw new Exception($"Нет курсов по валюте {request.CurrencyIdTo}");
                }

                coefficientResult.Coefficient = 1 / rate.Coefficient;
                result.Add(coefficientResult);
                continue;
            }

            var rateWithTargetCurrencyTo = ratesByDay.FirstOrDefault(r => r.Target == request.CurrencyIdFrom);
            // После реализации механизма прогрузки курсов для новой валюты это будет не нужно
            if (rateWithTargetCurrencyTo is null)
            {
                throw new Exception($"Нет курсов по валюте {request.CurrencyIdTo}");
            }

            var rateWithTargetCurrencyFrom = ratesByDay.FirstOrDefault(r => r.Target == request.CurrencyIdTo);
            // После реализации механизма прогрузки курсов для новой валюты это будет не нужно
            if (rateWithTargetCurrencyFrom is null)
            {
                throw new Exception($"Нет курсов по валюте {request.CurrencyIdTo}");
            }

            coefficientResult.Coefficient = rateWithTargetCurrencyFrom.Coefficient / rateWithTargetCurrencyTo.Coefficient;
            result.Add(coefficientResult);
        }

        return result.ToArray();
    }
}