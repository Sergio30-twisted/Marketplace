namespace Logisitica_Integrada.Models
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public List<int> ProductIds { get; set; } = new();
    }
}
