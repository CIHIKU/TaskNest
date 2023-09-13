using System.Text;
using AuthService.Services.Interfaces;
using RabbitMQ.Client;

namespace AuthService.Services;

public class RabbitMqPublisherService : IRabbitMqPublisherService
{
    private readonly IModel _channel;

    public RabbitMqPublisherService()
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

    public void Publish(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(
            exchange: "",
            routingKey: "AuthServiceQueue",
            basicProperties: null,
            body: body
        );
    }
}