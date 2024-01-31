using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using Telegram.Bot.Types;

namespace SpendingTracker.TelegramBot.Handlers.ProcessButtonClick.Contracts;

public class ProcessButtonClickCommand : ICommand
{
    public CallbackQuery CallbackQuery { get; init; }
}