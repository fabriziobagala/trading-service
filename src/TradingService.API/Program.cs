using System.Text.Json.Serialization;
using TradingService.API.Extensions;
using TradingService.Application;
using TradingService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog for logging.
builder.Host.UseSerilog();

// Configure API versioning and Swagger.
builder.Services.AddApiVersioningAndSwagger();

// Register the layer services.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Use lowercase URLs.
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

// Convert enums to strings in JSON responses.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds services for custom problem details.
builder.Services.AddCustomProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithVersioning();
}

app.UseUseExceptionHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
