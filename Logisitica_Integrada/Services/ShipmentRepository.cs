using Logisitica_Integrada.Models;

namespace Logisitica_Integrada.Services
{
    public class ShipmentRepository
    {
        private static readonly List<Shipment> _shipments = new();
        private static int _nextId = 1;

        public Shipment Add(Shipment shipment)
        {
            shipment.Id = _nextId++;
            _shipments.Add(shipment);
            return shipment;
        }

        public Shipment? GetByNumeroGuia(string numeroGuia)
        {
            return _shipments.FirstOrDefault(s =>
                s.NumeroGuia.Equals(numeroGuia, StringComparison.OrdinalIgnoreCase));
        }

        public Shipment? GetByOrderId(int orderId)
        {
            return _shipments.FirstOrDefault(s => s.OrderId == orderId);
        }

        public List<Shipment> GetAll()
        {
            return _shipments.ToList();
        }

        public bool UpdateEstado(string numeroGuia, string nuevoEstado)
        {
            var shipment = GetByNumeroGuia(numeroGuia);
            if (shipment == null) return false;

            shipment.Estado = nuevoEstado;
            shipment.FechaActualizacion = DateTime.UtcNow;
            return true;
        }
    }
}
