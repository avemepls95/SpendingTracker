using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SpendingTracker.Application;
using SpendingTracker.Application.CustomFilters;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.CurrencyRate;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;
using SpendingTracker.WebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigureServices(builder.Services);
    
var app = builder.Build();
app.RunStartupTasks();

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

static void ConfigureServices(IServiceCollection serviceCollection)
{
    var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
    var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
    var configuration = AppConfigurationBuilder.Build();

    serviceCollection
        .AddDispatcher(assembliesForScan)
        .AddFluentValidation(assembliesForScan)
        .AddGenericSubDomain(configuration)
        .AddInfrastructure(configuration)
        .AddApplicationLayer()
        .AddCurrencyRates()
        .AddMemoryCache()
        .AddLogging(configure => configure.AddConsole())
        .AddMvc()
        .AddNewtonsoftJson(SetJsonConfiguration);
    
    var oAuthOptions = configuration.GetSection(nameof(OAuthOptions)).Get<OAuthOptions>();
    serviceCollection.AddJwtBearerTokenAuth(oAuthOptions);

    serviceCollection.AddControllers();
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