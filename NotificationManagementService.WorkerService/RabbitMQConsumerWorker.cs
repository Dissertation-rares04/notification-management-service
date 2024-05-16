using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NotificationManagementService.Business.Interface;
using NotificationManagementService.Core.AppSettings;
using NotificationManagementService.Core.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationManagementService.WorkerService
{
    public class RabbitMQConsumerWorker : BackgroundService, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;

        private readonly IMessageHandler _messageHandler;
        private readonly ILogger<RabbitMQConsumerWorker> _logger;

        public RabbitMQConsumerWorker(IMessageHandler messageHandler, IOptions<RabbitMQSettings> rabbitMQSettings, ILogger<RabbitMQConsumerWorker> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;

            InitRabbitMQ(rabbitMQSettings);
        }

        private void InitRabbitMQ(IOptions<RabbitMQSettings> rabbitMQSettings)
        {
            _logger.LogInformation($"Setting up rabbitmq connection on {rabbitMQSettings.Value.Hostname}:{rabbitMQSettings.Value.Port}");

            //Here we specify the Rabbit MQ Server. we use rabbitmq docker image and use it
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQSettings.Value.Hostname,
                Port = rabbitMQSettings.Value.Port
            };

            //Create the RabbitMQ connection using connection factory details as i mentioned above
            _connection = factory.CreateConnection();

            //Here we create channel with session and model
            _channel = _connection.CreateModel();

            //declare the queue after mentioning name and a few property related to that
            _channel.QueueDeclare("notification_tasks", exclusive: false);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            //Set Event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageObj = JsonConvert.DeserializeObject<Message>(message);
                _messageHandler.HandleMessage(messageObj);

                _logger.LogInformation($"Consumed {message}");
            };

            //read the message
            _channel.BasicConsume(queue: "notification_tasks", autoAck: true, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_channel != null)
                {
                    _channel.Dispose();
                    _channel = null;
                }

                if (_connection != null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
    }
}
