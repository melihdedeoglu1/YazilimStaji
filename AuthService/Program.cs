using AuthService.Data;
using AuthService.Services;
using AuthService.Settings;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AuthContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuthServices, AuthServices>();

builder.Services.AddHttpClient<IAuthServices, AuthServices>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://localhost:7173") // frontend portu!
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});




builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton<JwtTokenGenerator>();



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



app.UseCors("AllowFrontend");



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
