using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace RabbitApi;

public interface IRabbitMqService
{
    IConnection CreateConnection();
}

public class RabbitMqService : IRabbitMqService
{
    private readonly RabbitMqConfiguration _configuration;
    public RabbitMqService(IOptions<RabbitMqConfiguration> options)
    {
        _configuration = options.Value;
    }

    public IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration.HostName,
            UserName = _configuration.UserName,
            Password = _configuration.Password
        };

        factory.DispatchConsumersAsync = true;
        factory.Uri = new Uri($"amqp://{_configuration.UserName}:{_configuration.Password}@{_configuration.HostName}");
        factory.ClientProvidedName = "RabbitMqSender";
        var channel = factory.CreateConnection();
        return channel;
    }
}
