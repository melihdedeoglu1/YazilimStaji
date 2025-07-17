using Microsoft.EntityFrameworkCore;
using System;
using UnitTestExamples.Data;
using UnitTestExamples.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UserContext>(opt =>
    opt.UseInMemoryDatabase("TestDb"));

builder.Services.AddScoped<IUserService, UserService>();



builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
