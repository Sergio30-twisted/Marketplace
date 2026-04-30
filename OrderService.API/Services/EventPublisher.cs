using Microsoft.AspNetCore.Connections;
using System.Text;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.API.Services;

public class EventPublisher
{
    public void Publicar(string evento)
    {
        try
        {
            Console.WriteLine("Conectando a RabbitMQ...");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "pedido-creado",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(evento);

            channel.BasicPublish(
                exchange: "",
                routingKey: "pedido-creado",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"Evento enviado a RabbitMQ: {evento}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ERROR RABBITMQ: {ex.Message}");
        }
    }
}