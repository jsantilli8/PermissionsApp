using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Application;
using Domain.Entities;
using Infrastructure.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Logging;
using Serilog;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration

// Configure SQL Server database
builder.Services.AddDbContext<PermissionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Dependency Injection

// Register services and repositories
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IElasticsearchRepository, ElasticsearchRepository>();
builder.Services.AddApplicationServices();

#endregion

#region Controllers

// Add controllers to the application
builder.Services.AddControllers();

#endregion

#region Logging (Serilog)

// Configure Serilog for logging
SerilogConfiguration.ConfigureLogging();
builder.Host.UseSerilog();
builder.Services.AddLoggingServices();

#endregion

#region Swagger Configuration

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Permissions API", Version = "v1" });
});

#endregion

#region Kafka Configuration

// Register Kafka Producer and Consumer
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddHostedService<KafkaConsumer>();

#endregion

#region Elasticsearch Configuration

#endregion

// Build the application
var app = builder.Build();

#region Middleware Configuration

// Enable Serilog request logging
app.UseSerilogRequestLogging();

// Apply database migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PermissionsDbContext>();
    dbContext.Database.Migrate();
}

// Configure Swagger for development environments
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Permissions API v1");
        c.RoutePrefix = string.Empty; 
    });
}

// Enable Authorization
app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();
