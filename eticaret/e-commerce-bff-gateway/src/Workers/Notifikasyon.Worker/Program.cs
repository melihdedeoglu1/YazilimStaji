using MassTransit;
using Notifikasyon.Worker.Consumers;

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
            })
            .Build();

        host.Run();
    }
}