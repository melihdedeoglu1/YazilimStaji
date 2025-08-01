using MassTransit;
using Microsoft.EntityFrameworkCore;
using Odeme.API.Data;
using Odeme.API.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OdemeContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddMassTransit(configurator =>
{

    configurator.AddConsumer<OdemeYapCommandConsumer>();
    configurator.AddConsumer<OdemeIadeEtCommandConsumer>();
    configurator.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });


        cfg.ReceiveEndpoint("odeme-yap-kuyrugu", e =>
        {
            e.ConfigureConsumer<OdemeYapCommandConsumer>(context);
        });
        cfg.ReceiveEndpoint("odeme-iade-et-kuyrugu", e =>
        {
            e.ConfigureConsumer<OdemeIadeEtCommandConsumer>(context);
        });
    });
});
builder.Services.AddHttpClient();

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
        var dbContext = services.GetRequiredService<OdemeContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.Run();
