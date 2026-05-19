using APIGateWay_MarketPlace.Hubs;
using APIGateWay_MarketPlace.Models;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace APIGateWay_MarketPlace.Services
{
    public class EventBusConsumer : BackgroundService
    {

        private readonly IHubContext<NotificacionesHub> _hub;

        public EventBusConsumer(IHubContext<NotificacionesHub> hub)
        {
            _hub = hub;
        }

        /*
        RabbitMQ (cola)
            ↓
        EventBusConsumer se suscribe y AGARRA el mensaje
            ↓
        Del mensaje extrae el userId (que el microservicio metió ahí)
            ↓
        HubContext busca al cliente con ese userId en los grupos
            ↓
        Le manda el mensaje por el WebSocket abierto 


         */


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // 1. Te conectas a RabbitMQ y te suscribes a la cola
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync();

          await  channel.QueueDeclareAsync(queue: "queue.pedidos",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false);

            // 2. Defines qué hacer cuando llega un mensaje
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                // 3. Agarras el mensaje de la cola
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                // 4. Lo deserializas para leer sus datos
                var evento = JsonSerializer.Deserialize<PedidoActualizadoEvent>(json);

                // 5. Usas el userId que venía dentro del mensaje
                // para mandarle la notificación solo a ese cliente
                await _hub.Clients.Group(evento.UsuarioId.ToString())
                    .SendAsync("PedidoActualizado", new
                    {
                        evento.PedidoId,
                        evento.Estado
                    });
            };

            // 6. Empiezas a escuchar (esto no se cierra nunca)
            await channel.BasicConsumeAsync(queue: "queue.pedidos",
                                 autoAck: true,
                                 consumer: consumer);

            // 7. Se queda vivo hasta que la app se apague
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
