using System.Text;
using AuthService.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuthService.Services;

public class RabbitMqConsumerService : IRabbitMqConsumerService
{
    private readonly IModel _channel;

    public RabbitMqConsumerService()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(
            queue: "AuthServiceQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }
    
    public void Consume()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };
        
        _channel.BasicConsume(
            queue: "AuthServiceQueue",
            autoAck: true,
            consumer: consumer
        );
    }
}