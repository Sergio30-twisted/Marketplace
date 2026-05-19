namespace Logisitica_Integrada.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public string NumeroGuia { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public List<int> ProductIds { get; set; } = new();
        public string Estado { get; set; } = "En preparación";
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
