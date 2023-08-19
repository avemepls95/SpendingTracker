using System.Reflection;
using SpendingTracker.Application;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.GenericSubDomain;
using SpendingTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var assemblyNamesForScan = new [] { "SpendingTracker.Application" };
var assembliesForScan = assemblyNamesForScan.Select(Assembly.Load).ToArray();
var configuration = AppConfigurationBuilder.Build();
builder.Services
    .AddDispatcher(assembliesForScan)
    .AddGenericSubDomain(configuration)
    .AddInfrastructure(configuration)
    .AddMemoryCache()
    .AddLogging(configure => configure.AddConsole());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();