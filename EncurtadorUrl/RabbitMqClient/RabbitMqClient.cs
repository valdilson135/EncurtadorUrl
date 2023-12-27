using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using EncurtadorUrl.Interfaces;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.RabbitMqClient
{
    public class RabbitMqClients : IRabbitMqClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;  

        public RabbitMqClients(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = Int32.Parse(_configuration["RabbitMqPort"]),
                UserName = _configuration["RabbitMqUser"],
                Password = _configuration["RabbitMqPassword"]
            }.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublicUrl(UrlModel urlCreate)
        {
            string mensagem = JsonSerializer.Serialize(urlCreate); 
            _channel.QueueDeclare(queue: mensagem,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }         
    }
}
