using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Services.Consumers
{
    public class DirectQueue1Consumer : IHostedService, IDisposable
    {
        private bool _running;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly string _queueName = "direct-q-1";

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _running = true;
            Task.Run(ListenForMessages, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            // Add code here to stop listening for messages
            _channel?.Close();
            _connection?.Close();
            return Task.CompletedTask;
        }

        public async Task ListenForMessages()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 30001 };
            _connection = factory.CreateConnection("RabbitMQ.Payground.DirectQueue1Consumer");
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();

            while (_running)
            {
                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    // Nack a message
                    //_channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);

                    // Reject a message
                    //_channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);

                    Console.WriteLine("Message received {0}", message);
                };

                _channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
