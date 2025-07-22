using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Adým 1: ocelot.json dosyasýný projenin yapýlandýrmasýna ekle.
// Bu sayede Ocelot, kurallarýný bu dosyadan okuyabilir.
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Adým 2: Ocelot servislerini Dependency Injection'a ekle.
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Adým 3: Ocelot'u uygulamanýn middleware'i (ara yazýlýmý) olarak kullan.
// Bu, gelen her isteðin Ocelot tarafýndan iþlenmesini saðlar.
await app.UseOcelot();

app.Run();