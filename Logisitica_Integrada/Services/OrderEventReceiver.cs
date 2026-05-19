using Logisitica_Integrada.Models;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Logisitica_Integrada.Services
{
    public class OrderEventReceiver : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IModel? _channel;

        public OrderEventReceiver(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[OrderEventReceiver] Iniciando escucha de la cola 'pedido-creado'...");

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(
                    queue: "pedido-creado",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (_, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var mensaje = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"[OrderEventReceiver] Mensaje recibido: {mensaje}");

                        OrderCreatedEvent? evento = null;

                        try
                        {
                            evento = JsonSerializer.Deserialize<OrderCreatedEvent>(mensaje,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        }
                        catch
                        {
                            // Compatibilidad con OrderService actual que publica sólo "PedidoCreado"
                            evento = new OrderCreatedEvent
                            {
                                OrderId = Random.Shared.Next(1000, 9999),
                                Cliente = "Cliente Desconocido",
                                Direccion = "Dirección no especificada",
                                ProductIds = new List<int> { 1 }
                            };
                        }

                        if (evento != null)
                        {
                            using var scope = _scopeFactory.CreateScope();
                            var manager = scope.ServiceProvider.GetRequiredService<ShipmentManager>();
                            manager.CrearEnvio(evento);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[OrderEventReceiver] ❌ Error al procesar mensaje: {ex.Message}");
                    }
                };

                _channel.BasicConsume(
                    queue: "pedido-creado",
                    autoAck: true,
                    consumer: consumer
                );

                Console.WriteLine("[OrderEventReceiver] ✅ Escuchando cola 'pedido-creado'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OrderEventReceiver] ⚠ RabbitMQ no disponible al iniciar: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
