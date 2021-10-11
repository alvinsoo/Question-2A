using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Question2Microservices_11_10_21.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Question2Microservices_11_10_21.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        [HttpPost]
        public void Post([FromBody] eptTask task)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 32461
                //HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                //Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
            };

            Console.WriteLine(factory.HostName + ":" + factory.Port);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "TaskQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                
                string message = "{\"token\": \"QpwL5tke4Pnpja7X4}";

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "TaskQueue",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
