using InvoiceEvaluationAPI.Services;
using Microsoft.OpenApi.Models;
using RestSharp;
using Serilog;

// Ensure Logs Directory Exists
Directory.CreateDirectory("logs");

Directory.CreateDirectory("uploads");

// Configure Serilog to Write to File
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // Also log to console
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // Log to file
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    // Set the port for your application, e.g., 5001
    serverOptions.ListenAnyIP(9001); // Or any IP address and port
});


//builder.Services.AddSingleton<IRestClient>(new RestClient("https://0b662f66-703a-4f48-b2c7-2c155e68b842.mock.pstmn.io/"));//for postman
builder.Services.AddSingleton<IRestClient>(new RestClient("http://localhost:5005/api/classification/")); //for local api service
builder.Services.AddSingleton<InvoiceClassificationService>();
builder.Services.AddSingleton<RuleEngineService>();

//  Add Serilog
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Invoice Evaluation API",
        Version = "v1",
        Description = "API to evaluate and classify invoices.",
        Contact = new OpenApiContact
        {
            Name = ":" + "  Narase Gowda Kumbaiah",
            Email = "narasegowdanithin@gmail.com"
        }
    });
});

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }