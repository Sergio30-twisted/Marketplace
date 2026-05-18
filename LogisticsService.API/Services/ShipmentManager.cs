using LogisticsService.API.Models;

namespace LogisticsService.API.Services;

public class ShipmentManager
{
    private readonly ShipmentRepository _repository;
    private readonly LogisticsNotifier _notifier;

    public ShipmentManager(ShipmentRepository repository, LogisticsNotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public Shipment CrearEnvio(OrderCreatedEvent evento)
    {
        var shipment = new Shipment
        {
            NumeroGuia = GenerarNumeroGuia(),
            OrderId = evento.OrderId,
            Cliente = evento.Cliente,
            Direccion = evento.Direccion,
            ProductIds = evento.ProductIds,
            Estado = "En preparación",
            FechaCreacion = DateTime.UtcNow
        };

        _repository.Add(shipment);

        Console.WriteLine($"[ShipmentManager] Envío creado — Guía: {shipment.NumeroGuia} | Orden: {shipment.OrderId}");

        _notifier.NotificarCambioEstado(shipment);

        return shipment;
    }

    public bool ActualizarEstado(string numeroGuia, string nuevoEstado)
    {
        var estadosValidos = new[] { "En preparación", "En camino", "Entregado" };

        if (!estadosValidos.Contains(nuevoEstado))
        {
            Console.WriteLine($"[ShipmentManager] Estado inválido: {nuevoEstado}");
            return false;
        }

        var actualizado = _repository.UpdateEstado(numeroGuia, nuevoEstado);

        if (!actualizado)
        {
            Console.WriteLine($"[ShipmentManager] Guía no encontrada: {numeroGuia}");
            return false;
        }

        var shipment = _repository.GetByNumeroGuia(numeroGuia)!;
        Console.WriteLine($"[ShipmentManager] Estado actualizado — Guía: {numeroGuia} | Nuevo estado: {nuevoEstado}");

        _notifier.NotificarCambioEstado(shipment);

        return true;
    }

    private static string GenerarNumeroGuia()
    {
        var fecha = DateTime.UtcNow.ToString("yyyyMMdd");
        var sufijo = Random.Shared.Next(1000, 9999);
        return $"GU-{fecha}-{sufijo}";
    }
}