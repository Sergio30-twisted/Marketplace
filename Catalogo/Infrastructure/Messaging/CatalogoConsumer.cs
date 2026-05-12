using Catalogo.Infrastructure.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Catalogo.Infrastructure.Messaging
{
    public class CatalogoConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;
        // Ajustado al nombre real que aparece en tu consola de RabbitMQ
        private const string QueueName = "queue.catalogo.crear";

        public CatalogoConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async Task InitRabbitMQAsync()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: QueueName,
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

            Console.WriteLine($"--> [CONEXIÓN] Conectado exitosamente a la cola: {QueueName}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Inicializamos la conexión antes de empezar a consumir
            await InitRabbitMQAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("--> [EVENTO] ¡Mensaje detectado en RabbitMQ!");

                try
                {
                    // Configuramos para ignorar diferencias entre mayúsculas/minúsculas del JSON del Front
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    var nuevoCatalogo = JsonSerializer.Deserialize<Catalogo.Domain.Entities.Catalogo>(message, options);

                    if (nuevoCatalogo != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var repository = scope.ServiceProvider.GetRequiredService<ICreacionCatalogoRepository>();

                        await repository.InsertarNuevoCatalogoAsync(nuevoCatalogo);

                        Console.WriteLine($"--> [ÉXITO] Catálogo '{nuevoCatalogo.NombreCatalogo}' guardado en MongoDB.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> [ERROR] Error al procesar mensaje: {ex.Message}");
                }
            };

            // Iniciamos el consumo
            await _channel.BasicConsumeAsync(queue: QueueName,
                                            autoAck: true, // Esto borrará el mensaje de RabbitMQ al leerlo
                                            consumer: consumer);

            Console.WriteLine("--> [ESCUCHA] Esperando nuevos mensajes...");

            // Mantenemos el servicio vivo
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken); // Revisión cada 5 segundos
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("--> [STOP] Deteniendo el consumidor de RabbitMQ...");
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}