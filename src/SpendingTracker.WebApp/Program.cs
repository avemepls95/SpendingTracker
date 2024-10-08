using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SpendingTracker.Application;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.CurrencyRate;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;
using SpendingTracker.WebApp;
using SpendingTracker.WebApp.CustomFilters;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

// Add services to the container.
ConfigureServices(builder);
    
var app = builder
    .Build()
    .InitializeData()
    .RunStartupTasks();

var cultureInfo = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

static void ConfigureServices(WebApplicationBuilder webApplicationBuilder)
{
    var serviceCollection = webApplicationBuilder.Services;
    
    var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
    var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
    var configuration = AppConfigurationBuilder.Build();

    var connectionStrings = ConfigurationReader.ReadConnectionStrings(configuration);
    var telegramOptions = ConfigurationReader.ReadTelegramOptions(configuration);

    serviceCollection
        .AddDispatcher(assembliesForScan)
        .AddFluentValidation(assembliesForScan)
        .AddGenericSubDomain(telegramOptions)
        .AddInfrastructure(connectionStrings)
        .AddApplicationLayer()
        .AddCurrencyRates()
        .AddMemoryCache()
        .AddLogging(configure => configure.AddConsole())
        .AddMvc()
        .AddNewtonsoftJson(SetJsonConfiguration);

    var oAuthOptions = ConfigurationReader.ReadOAuthOptions(configuration);
    serviceCollection.AddJwtBearerTokenAuth(oAuthOptions);

    serviceCollection.AddControllers(options => options.UseDateOnlyTimeOnlyStringConverters());
    serviceCollection.AddEndpointsApiExplorer();
    serviceCollection.AddSwaggerGen();
    serviceCollection.AddCors();
    serviceCollection.TryAddEnumerable(ServiceDescriptor.Transient<IStartupFilter, CorsStartupFilter>());
}

static void SetJsonConfiguration(MvcNewtonsoftJsonOptions options)
{
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
}