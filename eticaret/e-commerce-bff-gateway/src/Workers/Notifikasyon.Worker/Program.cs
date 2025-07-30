using MassTransit;
using Notifikasyon.Worker.Consumers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(configurator =>
                {

                    configurator.AddConsumer<SiparisOlusturulduConsumer>();

                    configurator.UsingRabbitMq((context, config) =>
                    {

                        var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMQ");
                        config.Host(rabbitMqConfig["Host"], "/", h =>
                        {
                            h.Username(rabbitMqConfig["Username"] ?? "guest");
                            h.Password(rabbitMqConfig["Password"] ?? "guest");
                        });


                        config.ReceiveEndpoint("siparis-olusturuldu-notifikasyon-kuyrugu", e =>
                        {
                            e.ConfigureConsumer<SiparisOlusturulduConsumer>(context);
                            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
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