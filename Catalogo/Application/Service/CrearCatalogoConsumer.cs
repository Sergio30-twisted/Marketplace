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
            // 1. Liberamos el hilo para que la Web API levante sus puertos sin congelarse
            await Task.Yield();

            var factory = new ConnectionFactory
            {
                HostName = "host.docker.internal",
                RequestedConnectionTimeout = TimeSpan.FromSeconds(10)
            };

            IConnection connection = null!;
            IChannel channel = null!;

            Console.WriteLine("🚀 Iniciando bucle de persistencia para RabbitMQ...");

            // 2. El único bucle encargado de conectar
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("🔄 Intentando conectar a RabbitMQ...");

                    connection = await factory.CreateConnectionAsync(stoppingToken);
                    channel = await connection.CreateChannelAsync();

                    if (connection != null && channel != null)
                    {
                        Console.WriteLine("✅ Conexión exitosa con RabbitMQ de manera segura.");
                        break; // Rompemos el bucle, todo un éxito
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al conectar a RabbitMQ: {ex.Message}.");
                    Console.WriteLine("⏳ Esperando 5 segundos para el próximo intento...");

                    try
                    {
                        await Task.Delay(5000, stoppingToken);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine("🛑 Detención detectada durante la espera.");
                        return;
                    }
                }
            }

            if (channel == null) return;

            // 3. Declaración de colas y consumidor (Solo se ejecuta si la conexión fue exitosa)
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
                Console.WriteLine($"📨 Mensaje recibido: {json}");

                var catalogo = JsonSerializer.Deserialize<Domain.Entities.Catalogo>(json,
                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (catalogo is not null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<IAdministracionDeCatalogosService>();
                    await service.CrearAsync(catalogo);
                    Console.WriteLine("✅ Guardado en MongoDB");
                }
            };

            await channel.BasicConsumeAsync(
                queue: "queue.catalogo.crear",
                autoAck: true,
                consumer: consumer);

            // Mantener vivo el servicio escuchando
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}