using Catalogo.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalogo.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Catalogo
    {
        [BsonId]
        [BsonSerializer(typeof(Int32Serializer))]
        [BsonElement("_id")]
        public int Id { get; set; }

        [BsonElement("nombreCatalogo")]
        public string NombreCatalogo { get; set; } = null!;

        [BsonElement("descripcionCatalogo")]
        public string DescripcionCatalogo { get; set; } = null!;

        [BsonElement("nombreProductor")]
        public string NombreProductor { get; set; }

        [BsonElement("telefonoContacto")]
        public string TelefonoContacto { get; set; }

        [BsonElement("Categoria")] // Asegúrate que la 'C' sea mayúscula como en tu foto
        [BsonRepresentation(BsonType.String)]
        public CategoriaCatalogo Categoria { get; set; }

        // Esta lista permite la acción de "meterle productos" al mismo componente
        // Al estar aquí, se guardan como documentos embebidos en la colección Products
        [BsonElement("productos")]
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
