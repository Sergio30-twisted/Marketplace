namespace APIGateWay_MarketPlace.Models
{
    public class PedidoActualizadoEvent
    {
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEvento { get; set; }
    }
}
