using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Rapor.API.Consumers;
using Rapor.API.Data;
using Rapor.API.Repositories;
using Rapor.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");




builder.Services.AddDbContext<RaporContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddMassTransit(configurator =>
{
    
    configurator.AddConsumer<SiparisOlusturulduConsumer>();
    configurator.AddConsumer<IadeOnaylandiConsumer>();

    configurator.UsingRabbitMq((context, cfg) =>
    {
        
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        
        cfg.ReceiveEndpoint("rapor-siparisolusturuldu-kuyrugu", e =>
        {
            e.ConfigureConsumer<SiparisOlusturulduConsumer>(context);
        });
        cfg.ReceiveEndpoint("rapor-iadeonaylandi-kuyrugu", e =>
        {
            e.ConfigureConsumer<IadeOnaylandiConsumer>(context);
        });
    });
});
 
builder.Services.AddScoped<IRaporRepository, RaporRepository>();
builder.Services.AddScoped<IRaporService, RaporService>();



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





/*
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<JwtTokenGenerator>();


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
                Encoding.UTF8.GetBytes("super-secret-simplemart-key-1234567890")) // auth servis ile ayn? key
        };
    });
*/
builder.Services.AddAuthorization();




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

//app.UseHttpsRedirection();

app.UseAuthorization();




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<RaporContext>();
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
