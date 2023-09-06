using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SpendingTracker.Application.CustomFilters
{
    public class CorsStartupFilter : IStartupFilter
    {
        private readonly IConfiguration _configuration;

        public CorsStartupFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                var corsOptions = _configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();

                app.UseCors(c => c
                    .WithOrigins(corsOptions.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

                next(app);
            };
        }
    }
}