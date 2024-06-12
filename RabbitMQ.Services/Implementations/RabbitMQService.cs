using Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Contracts;
using System.Text;

namespace Services.Implementations
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection? _connection;
        private readonly IModel? _channel;

        public RabbitMQService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost", Port = 30001 };
            _connection = _factory.CreateConnection("RabbitMQ.Payground");
            _channel = _connection.CreateModel();
            _channel.ConfirmSelect();

            SetupFanoutPublish();

            SetupDirectPublish();

            SetupTopicPublish();

            SetupHeadersPublish();

            _channel.BasicReturn += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Message returned: {0}", message);
            };
        }

        private void SetupDirectPublish()
        {
            // Declare the Direct Exchange
            _channel.ExchangeDeclare("direct-exchange", ExchangeType.Direct);

            // Declare the Direct Queues
            _channel.QueueDeclare("direct-q-1", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("direct-q-2", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Bind the Direct Queue to the Direct Exchange
            _channel.QueueBind("direct-q-1", "direct-exchange", routingKey: "d-q-1");
            _channel.QueueBind("direct-q-2", "direct-exchange", routingKey: "d-q-2");
        }

        private void SetupFanoutPublish()
        {
            // Declare the Fanout Exchange
            _channel.ExchangeDeclare("fanout-exchange", ExchangeType.Fanout);

            // Declare the Fanout Queues
            _channel.QueueDeclare("fanout-q-1", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("fanout-q-2", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Bind the Fanout Queue to the Fanout Exchange
            _channel.QueueBind("fanout-q-1", "fanout-exchange", routingKey: "doesn't matter");
            _channel.QueueBind("fanout-q-2", "fanout-exchange", routingKey: "doesn't matter");
        }

        private void SetupTopicPublish()
        {
            // Declare the Topic Exchange
            _channel.ExchangeDeclare("topic-exchange", ExchangeType.Topic);

            // Declare the Topic Queues
            _channel.QueueDeclare("topic-q-1", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("topic-q-2", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("topic-q-3", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("topic-q-4", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Bind the Topic Queue to the Topic Exchange
            _channel.QueueBind("topic-q-1", "topic-exchange", routingKey: "hot.topic");
            _channel.QueueBind("topic-q-2", "topic-exchange", routingKey: "hot.topic");
            _channel.QueueBind("topic-q-3", "topic-exchange", routingKey: "boring.topic");
            _channel.QueueBind("topic-q-4", "topic-exchange", routingKey: "#.topic");
        }

        private void SetupHeadersPublish()
        {
            // Declare the Headers Exchange
            _channel.ExchangeDeclare("headers-exchange", ExchangeType.Headers);

            // Declare the Queues
            _channel.QueueDeclare("headers-q-1", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare("headers-q-2", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Bind the Queues to the Headers Exchange with headers
            var headers1 = new Dictionary<string, object> { { "x-match", "any" }, { "extension", "pdf" }, { "category", "report" } };
            var headers2 = new Dictionary<string, object> { { "format", "csv" }, { "type", "data" } };

            _channel.QueueBind("headers-q-1", "headers-exchange", routingKey: string.Empty, arguments: headers1);
            _channel.QueueBind("headers-q-2", "headers-exchange", routingKey: string.Empty, arguments: headers2);
        }

        public void DirectPublish(Message message, string routingKey)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            _channel.BasicPublish(exchange: "direct-exchange", routingKey: routingKey, basicProperties: null, mandatory: true, body: body);
        }

        public void FanoutPublish(Message message)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            _channel.BasicPublish(exchange: "fanout-exchange", routingKey: string.Empty, basicProperties: null, body: body);
        }

        public void TopicPublish(Message message, string topicName)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);
            _channel.BasicPublish(exchange: "topic-exchange", routingKey: topicName, basicProperties: null, mandatory: true, body: body);
        }

        public void HeadersPublish(Message message, Dictionary<string, object> headers)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var properties = _channel.CreateBasicProperties();
            properties.Headers = headers;

            _channel.BasicPublish(exchange: "headers-exchange", routingKey: string.Empty, basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
