namespace AuthService.Services.Interfaces;

public interface IRabbitMqPublisherService
{
    void Publish(string message);
}