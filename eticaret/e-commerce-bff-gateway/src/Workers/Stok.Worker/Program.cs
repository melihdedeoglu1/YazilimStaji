using MassTransit;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Stok.Worker.Consumers;
using Stok.Worker.Data;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                
                var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ProductContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                });

                services.AddMassTransit(configurator =>
                {
                    //configurator.AddConsumer<SiparisOlusturulduStokConsumer>();
                    configurator.AddConsumer<StokAyirCommandConsumer>();
                    configurator.AddConsumer<StokSerbestBirakCommandConsumer>();

                    configurator.UsingRabbitMq((context, config) =>
                    {
                        var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMQ");
                        config.Host(rabbitMqConfig["Host"], "/", h =>
                        {
                            h.Username(rabbitMqConfig["Username"] ?? "guest");
                            h.Password(rabbitMqConfig["Password"] ?? "guest");
                        });
                        /*
                        config.ReceiveEndpoint("siparis-olusturuldu-stok-kuyrugu", e =>
                        {
                            e.ConfigureConsumer<SiparisOlusturulduStokConsumer>(context);
                            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        });
                        */
                        config.ReceiveEndpoint("stok-ayir-kuyrugu", e =>
                        {
                            e.ConfigureConsumer<StokAyirCommandConsumer>(context);
                        });
                        config.ReceiveEndpoint("stok-serbest-birak-kuyrugu", e =>
                        {
                            e.ConfigureConsumer<StokSerbestBirakCommandConsumer>(context);
                        });
                    });
                });

               
                services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService(serviceName: hostContext.HostingEnvironment.ApplicationName))
                    .WithTracing(tracing =>
                    {
                        tracing                           
                            .AddHttpClientInstrumentation()                         
                            .AddSource("MassTransit");

                        
                        var otlpEndpoint = hostContext.Configuration["Otlp:Endpoint"];
                        if (!string.IsNullOrEmpty(otlpEndpoint))
                        {
                            tracing.AddOtlpExporter(otlpOptions =>
                            {
                                otlpOptions.Endpoint = new Uri(otlpEndpoint);
                            });
                        }
                    });
            })
            .Build();

        host.Run();
    }
}
