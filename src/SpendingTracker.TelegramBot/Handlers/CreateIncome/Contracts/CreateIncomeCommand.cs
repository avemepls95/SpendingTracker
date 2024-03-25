using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using Telegram.Bot.Types;

namespace SpendingTracker.TelegramBot.Handlers.CreateIncome.Contracts;

public class CreateIncomeCommand : ICommand
{
    public long TelegramUserId { get; set; }
    public Message Message { get; set; }
}