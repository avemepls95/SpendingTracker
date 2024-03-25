using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using Telegram.Bot.Types;

namespace SpendingTracker.TelegramBot.Handlers.ProcessStart.Contracts;

public class ProcessStartCommandCommand : ICommand
{
    public User TelegramUser { get; init; }
    
}