using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenTelemetry and configure it to use Azure Monitor.
builder.Services
    .AddOpenTelemetry()
    .WithTracing(builder =>
        builder.AddAspNetCoreInstrumentation((options) =>
            options.Filter = (httpContext) =>
            {
                var path = httpContext.Request.Path.Value ?? string.Empty;

                // Only allow API paths like /api/products, /api/users/1.
                // Filter out unwanted telemetry — especially HTTP request logs for static files.
                // Requests to /favicon.ico, /swagger, and other non-API paths will be excluded
                // from Azure Application Insights to avoid cluttering logs with irrelevant data.
                if (path.StartsWith("/api/"))
                {
                    return true;
                }

                return false;
            }
        )
    )
    .UseAzureMonitor();

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
