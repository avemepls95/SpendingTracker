using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.DataTransfer.Dispatcher.Interfaces;
using SpendingTracker.Infrastructure.Abstractions.Models.Stored;

namespace SpendingTracker.Application.Handlers.Auth.CreateAuthLog.Contracts;

public class CreateAuthLogCommand : ICommand
{
    public UserKey UserId { get; init; }
    public AuthSource Source { get; init; }
    public object? AdditionalData { get; init; }
}