using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowApp.Infrastructure.MessageQueue
{
    public class RabbitMQProducer
    {
        public async Task PublishMessageAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            // Criação da conexão de forma assíncrona (aguardando a conexão ser estabelecida)
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();        

            // Declaração da fila para garantir que ela exista
            channel.QueueDeclare(queue: "transactionQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            // Convertendo a mensagem para bytes
            var body = Encoding.UTF8.GetBytes(message);

            // Publicando a mensagem na fila
            channel.BasicPublish(exchange: "",
                                 routingKey: "transactionQueue",
                                 basicProperties: null,
                                 body: body);
        }
    }
}
