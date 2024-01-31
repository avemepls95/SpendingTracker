using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using Telegram.Bot.Types;

namespace SpendingTracker.TelegramBot.Handlers.CreateSpending.Contracts;

public class CreateSpendingCommand : ICommand
{
    public long TelegramUserId { get; set; }
    public Message Message { get; set; }
}