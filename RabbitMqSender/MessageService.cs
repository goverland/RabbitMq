using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer;

public interface IMessageService
{
    void ReadMessage();
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
        _model.BasicQos(0, 1, false);
    }

    public void ReadMessage()
    {
        var consumer = new EventingBasicConsumer(_model);
        consumer.Received += (sender, args) =>
        {
            var body = args.Body.ToArray();
            var text = Encoding.UTF8.GetString(body);
            Console.WriteLine(text);

            _model.BasicAck(args.DeliveryTag, false);
        };

        string consumerTag = _model.BasicConsume(_queueName, false, consumer);

        Console.ReadLine();

        _model.BasicCancel(consumerTag);

        _model.Close();
        _connection.Close();
    }
}
