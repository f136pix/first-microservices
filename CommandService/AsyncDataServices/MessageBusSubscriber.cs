using System.Text;
using System.Threading.Channels;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService // singleton running during app lifetime
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private string _queueNameLog;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;

        InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMqHost"], Port = int.Parse(_configuration["RabbitMqPort"]) };


        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

        // queue for platservice
        _queueName = _channel.QueueDeclare(queue: "asyncBus", durable: true, exclusive: false, autoDelete: false).QueueName;
        _channel.QueueBind(
            queue: _queueName,
            exchange: "trigger",
            routingKey: "");

        // queue for logService
        _queueNameLog = _channel.QueueDeclare(queue: "asyncBusLog", durable: true, exclusive: false, autoDelete: false).QueueName;
        _channel.QueueBind(
            queue: _queueNameLog,
            exchange: "trigger",
            routingKey: "");

        Console.WriteLine("--> Listening the message buses");
        _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        // consumer for asyncBusQueue
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {

            Console.WriteLine("--> Event recieved from Dotnet Service");

            var msgBody = ea.Body;
            var notificationMsg = Encoding.UTF8.GetString(msgBody.ToArray());

            _eventProcessor.ProcessEvent(notificationMsg);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

        // consume for asyncBusLogQueue
        /*var logConsumer = new EventingBasicConsumer(_channel);
        
        logConsumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event recieved from PHP Service");
        };
        _channel.BasicConsume(queue: _queueNameLog, autoAck: true, consumer: logConsumer);
        */

        return Task.CompletedTask;
    }
    private void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMq conn shut down");
    }

    public override void Dispose() // executed when this service class is stopped   
    {
        if (_channel.IsOpen) // cleanup of the rabbitmq connection
        {
            _channel.Close();
            _connection.Close();
        }
    }


}