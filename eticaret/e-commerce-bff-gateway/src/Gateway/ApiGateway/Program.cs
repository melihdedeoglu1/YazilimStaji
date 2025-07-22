using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Ad�m 1: ocelot.json dosyas�n� projenin yap�land�rmas�na ekle.
// Bu sayede Ocelot, kurallar�n� bu dosyadan okuyabilir.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Ad�m 2: Ocelot servislerini Dependency Injection'a ekle.
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Ad�m 3: Ocelot'u uygulaman�n middleware'i (ara yaz�l�m�) olarak kullan.
// Bu, gelen her iste�in Ocelot taraf�ndan i�lenmesini sa�lar.
await app.UseOcelot();

app.Run();