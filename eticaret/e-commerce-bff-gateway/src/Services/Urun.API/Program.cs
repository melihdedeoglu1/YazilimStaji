using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Urun.API.Data;
using Urun.API.DTOs;
using Urun.API.Repositories;
using Urun.API.Services;
using Urun.API.Validators;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UrunContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();





builder.Services.AddScoped<IValidator<ProductForCreateDto>, ProductForCreateDtoValidator>();
builder.Services.AddScoped<IValidator<ProductForUpdateDto>, ProductForUpdateDtoValidator>();

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
        var dbContext = services.GetRequiredService<UrunContext>();
        
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the urun_db database.");
    }
}




app.MapControllers();

app.Run();
