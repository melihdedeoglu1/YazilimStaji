using Iade.API.Consumers;
using Iade.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");



builder.Services.AddDbContext<IadeContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddMassTransit(configurator =>
{
    
    configurator.AddConsumer<SiparisIadeTalebiConsumer>(); 

    configurator.UsingRabbitMq((context, cfg) =>
    {
        
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        
        cfg.ReceiveEndpoint("iade-talebi-kuyrugu", e =>
        {
            e.ConfigureConsumer<SiparisIadeTalebiConsumer>(context);
        });
    });
});

builder.Services.AddHttpClient();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName: builder.Environment.ApplicationName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSource("MassTransit")

        .AddOtlpExporter(otlpOptions =>
        {
            var otlpEndpoint = builder.Configuration["Otlp:Endpoint"];
            if (!string.IsNullOrEmpty(otlpEndpoint))
            {
                otlpOptions.Endpoint = new Uri(otlpEndpoint);
            }
        }));



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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<IadeContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.MapControllers();

app.Run();
