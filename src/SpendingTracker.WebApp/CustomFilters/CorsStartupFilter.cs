namespace SpendingTracker.WebApp.CustomFilters
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
                var corsOptions = ConfigurationReader.ReadCorsOptions(_configuration);

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