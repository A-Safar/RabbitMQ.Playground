using Microsoft.AspNetCore.Mvc;
using Domain;
using Services.Contracts;

namespace RabbitMQ.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMQController(IRabbitMQService rabbitMQService) : ControllerBase
    {
        [HttpPost]
        [Route("direct")]
        public ActionResult Direct(Message message, string routingKey)
        {
            rabbitMQService.DirectPublish(message, routingKey);

            return Ok("Message published successfully!");
        }

        [HttpPost]
        [Route("fanout")]

        public ActionResult Fanout(Message message)
        {
            rabbitMQService.FanoutPublish(message);

            return Ok("Message published successfully!");
        }

        [HttpPost]
        [Route("topic")]

        public ActionResult Topic(Message message, string topicName)
        {
            rabbitMQService.TopicPublish(message, topicName);

            return Ok("Message published successfully!");
        }

        [HttpPost]
        [Route("headers")]

        public ActionResult Headers(Message message)
        {
            var headers = new Dictionary<string, object>
            {
                {"extension", "pdf"},
                {"format", "csv"},
                {"type","data"}
            };
            rabbitMQService.HeadersPublish(message, headers);

            return Ok("Message published successfully!");
        }
    }
}
