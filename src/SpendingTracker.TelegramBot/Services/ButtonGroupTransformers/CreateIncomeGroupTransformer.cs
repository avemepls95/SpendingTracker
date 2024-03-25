using System.Text;
using SpendingTracker.Domain.Accounts;
using SpendingTracker.GenericSubDomain.Common;
using SpendingTracker.GenericSubDomain.User.Abstractions;
using SpendingTracker.Infrastructure.Abstractions.Repositories;
using SpendingTracker.TelegramBot.Internal.Buttons;
using SpendingTracker.TelegramBot.Services.ButtonGroupTransformers.Abstractions;

namespace SpendingTracker.TelegramBot.Services.ButtonGroupTransformers;

public class CreateIncomeGroupTransformer : IButtonsGroupTransformer
{
    private readonly ITelegramUserIdStore _telegramUserIdStore;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyRepository _currencyRepository;

    public CreateIncomeGroupTransformer(
        ITelegramUserIdStore telegramUserIdStore,
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        ICurrencyRepository currencyRepository)
    {
        _telegramUserIdStore = telegramUserIdStore;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _currencyRepository = currencyRepository;
    }

    public async Task Transform(ButtonGroup createIncomeGroup, int? returnGroupId, CancellationToken cancellationToken)
    {
        var telegramUserId = _telegramUserIdStore.Id!.Value;
        var user = await _userRepository.GetByTelegramId(telegramUserId, cancellationToken);

        var userOrderedAccounts = await _accountRepository.GetOrderedByDateUserAccounts(user.Id, cancellationToken);

        var textBuilder = new StringBuilder(@"Введите доход в следующем формате:

Cумма
Описание
Дата (опционально)");

        if (userOrderedAccounts.Any())
        {
            textBuilder.AppendLine("Порядковый номер счета (опционально)");
            textBuilder.AppendLine();

            var accountsCurrencies = userOrderedAccounts.Select(a => a.CurrencyId).ToArray();
            var currencies = await _currencyRepository.GetByIds(accountsCurrencies, cancellationToken);
            
            for (var i = 0; i < userOrderedAccounts.Length; ++i)
            {
                var account = userOrderedAccounts[i];
                var currency = currencies.First(c => c.Id == account.CurrencyId);
                textBuilder.AppendLine(
                    $"{i + 1} - {account.Name} {currency.CountryIcon}{currency.Code} ({EnumHelper<AccountTypeEnum>.GetDisplayValue(account.Type)})");
            }
        }

        createIncomeGroup.SetText(textBuilder.ToString());
    }
}