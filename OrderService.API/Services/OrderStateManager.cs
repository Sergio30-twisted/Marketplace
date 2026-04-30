using OrderService.API.Models;

namespace OrderService.API.Services;

public class OrderStateManager
{
    public void ActualizarEstado(Order order, string estado)
    {
        order.Estado = estado;
    }
}