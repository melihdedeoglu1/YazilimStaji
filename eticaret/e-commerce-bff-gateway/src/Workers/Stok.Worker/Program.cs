// Stok.Worker/Program.cs

using MassTransit;
using Microsoft.EntityFrameworkCore; 
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
                    
                    configurator.AddConsumer<SiparisOlusturulduStokConsumer>();

                    configurator.UsingRabbitMq((context, config) =>
                    {
                        var rabbitMqConfig = hostContext.Configuration.GetSection("RabbitMQ");
                        config.Host(rabbitMqConfig["Host"], "/", h =>
                        {
                            h.Username(rabbitMqConfig["Username"]);
                            h.Password(rabbitMqConfig["Password"]);
                        });

                        config.ReceiveEndpoint("siparis-olusturuldu-stok-kuyrugu", e =>
                        {
                            e.ConfigureConsumer<SiparisOlusturulduStokConsumer>(context);
                            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                        });
                    });
                });
            })
            .Build();

        host.Run();
    }
}