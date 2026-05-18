using LogisticsService.API.Models;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LogisticsService.API.Services;

public class LogisticsNotifier
{
    public void NotificarCambioEstado(Shipment shipment)
    {
        try
        {
            Console.WriteLine($"[LogisticsNotifier] Publicando cambio de estado en RabbitMQ...");

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "logistica-estado",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var payload = new
            {
                NumeroGuia = shipment.NumeroGuia,
                OrderId = shipment.OrderId,
                Cliente = shipment.Cliente,
                Estado = shipment.Estado,
                Timestamp = DateTime.UtcNow
            };

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));

            channel.BasicPublish(
                exchange: "",
                routingKey: "logistica-estado",
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"[LogisticsNotifier] Evento publicado: Guía {shipment.NumeroGuia} → {shipment.Estado}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LogisticsNotifier] ⚠ RabbitMQ no disponible: {ex.Message}");
        }
    }
}