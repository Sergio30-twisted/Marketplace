using OrderService.API.Models;

namespace OrderService.API.Services;

public class OrderValidator
{
    public bool Validar(Order order)
    {
        if (order.Cantidad <= 0)
            return false;

        return true;
    }
}