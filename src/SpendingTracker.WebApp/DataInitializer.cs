using SpendingTracker.Infrastructure.Abstractions;

namespace SpendingTracker.WebApp;

public static class DataInitializer
{
    public static WebApplication InitializeData(this WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        using var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
        dataInitializer.Initialize();

        return webApp;
    }
}