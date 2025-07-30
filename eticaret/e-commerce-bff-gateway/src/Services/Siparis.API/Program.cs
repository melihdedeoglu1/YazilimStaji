using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Siparis.API.Consumers;
using Siparis.API.Data;
using Siparis.API.Repositories;
using Siparis.API.Services;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var jwtSettings = builder.Configuration.GetSection("JwtSettings"); 
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("super-secret-simplemart-key-1234567890"))
        };
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


// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SiparisContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();











builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddMassTransit(configurator =>
{
   
    configurator.AddConsumer<StokGuncellemeBasarisizConsumer>();
    configurator.AddConsumer<IadeOnaylandiConsumer>();

    
    configurator.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ");
        cfg.Host(rabbitMqConfig["Host"], "/", h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? "guest");
            h.Password(rabbitMqConfig["Password"] ?? "guest");
        });

        
        cfg.ReceiveEndpoint("stok-guncelleme-basarisiz-siparis-kuyrugu", e =>
        {
            e.ConfigureConsumer<StokGuncellemeBasarisizConsumer>(context);
        });

        cfg.ReceiveEndpoint("iade-talebi-kuyrugu-siparis", e =>
        {
            e.ConfigureConsumer<IadeOnaylandiConsumer>(context);
        });
    });
});



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
        var dbContext = services.GetRequiredService<SiparisContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the siparis_db database.");
    }
}









app.MapControllers();

app.Run();
