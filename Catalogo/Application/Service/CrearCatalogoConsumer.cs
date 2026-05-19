using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using Catalogo.Application.Interface;

namespace Catalogo.Application.Service
{
    public class CrearCatalogoConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public CrearCatalogoConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = await factory.CreateConnectionAsync(stoppingToken);
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "queue.catalogo.crear",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                Console.WriteLine($"📨 Mensaje recibido: {json}"); // ← agrega esto

                var catalogo = JsonSerializer.Deserialize<Domain.Entities.Catalogo>(json,
                     new JsonSerializerOptions
                         {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() } // ← agrega esto
                     });

                Console.WriteLine($"📦 Deserializado: {catalogo?.NombreCatalogo ?? "NULL"}"); // ← y esto

                if (catalogo is not null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.ServiceProvider
                        .GetRequiredService<IAdministracionDeCatalogosService>();
                    await service.CrearAsync(catalogo);
                    Console.WriteLine("✅ Guardado en MongoDB"); // ← y esto
                }
                else
                {
                    Console.WriteLine("❌ catalogo es NULL, no se guardó"); // ← y esto
                }
            };

            await channel.BasicConsumeAsync(
                queue: "queue.catalogo.crear",
                autoAck: true,
                consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

    }
}
