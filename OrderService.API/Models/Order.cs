namespace OrderService.API.Models;

public class Order
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Cantidad { get; set; }
    public string Estado { get; set; }
}