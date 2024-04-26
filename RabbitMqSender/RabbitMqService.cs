using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMqConsumer.Models;

namespace RabbitMqConsumer;

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
        var factory = new ConnectionFactory()
        {
            HostName = _configuration.HostName,
            UserName = _configuration.UserName,
            Password = _configuration.Password,

            Port = 5671,

            Ssl = new SslOption
            {
                CertPath = _configuration.CertPath,
                CertPassphrase = _configuration.CertPassword,
                Enabled = true,
                ServerName = _configuration.ServerName
            }
        };

        factory.ClientProvidedName = "RabbitMqSender";

        var channel = factory.CreateConnection();
        return channel;
    }
}


