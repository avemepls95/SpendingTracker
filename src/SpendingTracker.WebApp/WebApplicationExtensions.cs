using System.Diagnostics.CodeAnalysis;
using SpendingTracker.GenericSubDomain.StartupTasks.Abstractions;

namespace SpendingTracker.WebApp
{
    [ExcludeFromCodeCoverage]
    public static class WebApplicationExtensions
    {
        public static WebApplication RunStartupTasks(this WebApplication webApplication)
        {
            var startupTasks = webApplication.Services.GetServices<IStartupTask>();

            foreach (var startupTask in startupTasks)
            {
                try
                {
                    startupTask.Execute();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return webApplication;
        }
    }
}
