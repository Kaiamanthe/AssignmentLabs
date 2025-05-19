using Microsoft.Extensions.DependencyInjection;
using AssignmentLibrary.Core.Interfaces;
using AssignmentLibrary.Core.Services;
using AssignmentLibrary.UI;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IAssignmentService, AssignmentService>();
builder.Services.AddSingleton<IAppLogger, ConsoleAppLogger>();
builder.Services.AddSingleton<IAssignmentFormatter, AssignmentFormatter>();
builder.Services.AddSingleton<ConsoleUI>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
    // This class is used for testing purpose only.
    // It is not part of the actual application.
    // The main entry point for the application is in the Program.cs file.
}