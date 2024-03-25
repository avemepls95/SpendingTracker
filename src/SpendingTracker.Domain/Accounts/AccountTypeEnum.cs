using System.ComponentModel.DataAnnotations;

namespace SpendingTracker.Domain.Accounts;

public enum AccountTypeEnum
{
    None = 0,

    [Display(Name = "Дебетовая карта")]
    DebitCard,

    [Display(Name = "Кредитная карта")]
    CreditCard,

    [Display(Name = "Наличные")]
    Cash,

    [Display(Name = "Брокерский счет")]
    Brokerage,

    [Display(Name = "Другое")]
    Other,
}