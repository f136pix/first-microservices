using System.Text;
using System.Text.Json;
using PlatService.DTOs;
using RabbitMQ.Client;

namespace PlatService.AsyncDataServices.MessageBus;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _configuration;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration configuration)
    {

        _configuration = configuration;
        Console.WriteLine(_configuration["RabbitMqPort"]);
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMqHost"],
            Port = int.Parse(_configuration["RabbitMqPort"])
        };

        try
        {
            // connecting to mq messager
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: "asyncBus", durable: true, exclusive: false, autoDelete: false);
            _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown; // on shutdown
            Console.WriteLine("--> Connected to RabbitMQ");
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Error connecting to the BUS: {e.Message}");
        }
    }

    private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMqConnection shut");
    }

    private void SendMessage(string message)
    {
        var msgBody = Encoding.UTF8.GetBytes(message); // formatting message to bynary
        _channel.BasicPublish(
            exchange: "trigger",
            routingKey: "",
            basicProperties: null,
            body: msgBody);
        Console.WriteLine($"--> Sent message: {message}");
    }
    
    // closes channel/ connection
    public void Dispose()
    {
        Console.WriteLine("--> Message bus disposed");
        if(_channel.IsOpen) // cleanup
        {
            _channel.Close();
            _connection.Close();
        }
    }

    public void PublishNewPlatform(PlatformPublishDto platformPublishDto)
    {
        var message = JsonSerializer.Serialize(platformPublishDto);

        if (!_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMq connection closed");
            return;
        }

        Console.WriteLine("--> RabbitMq connection OK, sending message");
        SendMessage(message);
    }
}