using Domain;

namespace Services.Contracts
{
    public interface IRabbitMQService
    {
        public void DirectPublish(Message message, string routingKey);
        public void TopicPublish(Message message, string topicName);
        public void FanoutPublish(Message message);
        public void HeadersPublish(Message message, Dictionary<string, object> headers);
    }
}
