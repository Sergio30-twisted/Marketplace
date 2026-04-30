using OrderService.API.Models;

namespace OrderService.API.Services;

public class OrderProcessor
{
    private readonly OrderValidator _validator;
    private readonly InventoryService _inventory;
    private readonly PaymentOrchestrator _payment;
    private readonly OrderStateManager _state;
    private readonly EventPublisher _events;

    public OrderProcessor(
        OrderValidator validator,
        InventoryService inventory,
        PaymentOrchestrator payment,
        OrderStateManager state,
        EventPublisher events)
    {
        _validator = validator;
        _inventory = inventory;
        _payment = payment;
        _state = state;
        _events = events;
    }

    public async Task<string> ProcesarOrden(Order order)
    {
        // 1. Validar
        if (!_validator.Validar(order))
            return "Orden inválida";

        _state.ActualizarEstado(order, "Validado");

        // 2. Reservar
        var reservado = await _inventory.ReservarProducto(order.ProductId, order.Cantidad);

        if (!reservado)
            return "Sin stock disponible";

        _state.ActualizarEstado(order, "Reservado");

        // 3. Pago
        var pago = _payment.ProcesarPago();

        if (!pago)
            return "Error en pago";

        _state.ActualizarEstado(order, "Confirmado");

        // 4. Evento
        _events.Publicar("PedidoCreado");

        return "Orden completada";
    }
}