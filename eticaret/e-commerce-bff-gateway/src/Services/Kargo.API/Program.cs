using Kargo.API.Consumers;
using Kargo.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<KargoContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<KargoHazirlaCommandConsumer>();

    configurator.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbitMqConfig["Host"], "/", h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? "guest");
            h.Password(rabbitMqConfig["Password"] ?? "guest");
        });

       
        cfg.ReceiveEndpoint("kargo-hazirla-kuyrugu", e =>
        {
            e.ConfigureConsumer<KargoHazirlaCommandConsumer>(context);
        });
    });
});


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

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<KargoContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}



app.Run();
