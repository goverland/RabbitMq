using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMqConsumer;
using RabbitMqConsumer.Models;


await CreateHostBuilder(args).RunConsoleAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .UseConsoleLifetime()
               .ConfigureServices((hostContext, services) =>
               {
                   services.Configure<RabbitMqConfiguration>(a =>
                       hostContext.Configuration.GetSection(nameof(RabbitMqConfiguration)));

                   services.AddSingleton<IRabbitMqService, RabbitMqService>();
                   services.AddSingleton<IMessageService, MessageService>();
                   services.AddHostedService<ConsumerHostedService>();
                   services.AddSingleton(Console.Out);
               });
