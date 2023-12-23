using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitApi;

public interface IMessageService
{
    void SendMessage(string message);
}

public class MessageService : IMessageService
{
    private readonly IModel _model;
    private readonly IConnection _connection;

    const string _queueName = "User";
    const string _routingKey = "demo-route";
    const string _exchangeName = "UserExchange";

    public MessageService(IRabbitMqService connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        _model.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
        _model.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, null);
        _model.QueueBind(_queueName, _exchangeName, _routingKey, null);
    }
    public void SendMessage(string message)
    {
        var messageBody = Encoding.UTF8.GetBytes(message);
        _model.BasicPublish(_exchangeName, _routingKey, null, messageBody);

        _model.Close();
        _connection.Close();
    }
}
