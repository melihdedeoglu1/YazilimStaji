using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



builder.Services.AddHttpClient("ApiGatewayClient", client =>
{
    // docker-compose içindeki gateway'in adresi
    client.BaseAddress = new Uri("http://api-gateway:8080");
})
.AddPolicyHandler(GetRetryPolicy());


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

app.Run();



static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
   
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) 
        .WaitAndRetryAsync(3, retryAttempt => 
        {
            
            var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
            Console.WriteLine($"Tekrar denenecek... Bekleme süresi: {timeToWait.TotalSeconds}s");
            return timeToWait;
        }
        );
}
