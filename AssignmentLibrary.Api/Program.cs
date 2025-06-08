using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.UI;
using AssignmentLibrary.UI.UiUtilities;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

/// <summary>
/// Entry point for the AssignmentLibrary API application.
/// Configures services, middleware, and endpoints.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Adds controller for JSON serialization to use string values for enums.
/// </summary>
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

/// <summary>
/// Adds API support and Swagger generation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Registers OpenAPI.
/// </summary>
/// <remarks>
/// Uses <c>Microsoft.AspNetCore.OpenApi</c> extensions.
/// </remarks>
builder.Services.AddOpenApi();

/// <summary>
/// Register services for dependency injection.
/// </summary>
builder.Services.AddSingleton<IAssignmentService, AssignmentService>();
builder.Services.AddSingleton<IAppLogger, ConsoleAppLogger>();
builder.Services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
builder.Services.AddSingleton<ConsoleUI>();

var app = builder.Build();

/// <summary>
/// Configures the HTTP request pipeline.
/// Only enables Swagger UI in development.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// <summary>
/// Enables authorization middleware.
/// </summary>
app.UseAuthorization();

/// <summary>
/// Maps controller routes to endpoints.
/// </summary>
app.MapControllers();

/// <summary>
/// Starts the web application.
/// </summary>
app.Run();

/// <summary>
/// Program class use for integration testing and test host setup.
/// </summary>

public partial class Program
{
    // This class is used for testing purposes only.
}
