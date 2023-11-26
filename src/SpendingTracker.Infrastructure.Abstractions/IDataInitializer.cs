namespace SpendingTracker.Infrastructure.Abstractions;

public interface IDataInitializer : IDisposable
{
    void Initialize();
}