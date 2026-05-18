namespace APIGateWay_MarketPlace.Models
{
    public class CatalogoDto
    {
        public int Id { get; set; }
        public string NombreCatalogo { get; set; }
        public string DescripcionCatalogo { get; set; }
        public string NombreProductor { get; set; }
        public string TelefonoContacto { get; set; }
        public string Categoria { get; set; }
        public List<ProductoDto> Productos { get; set; }
    }
}
