using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalogo.API.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Catalogo
    {
        [BsonId]
        [BsonSerializer(typeof(Int32Serializer))]
        public int Id { get; set; }

        [BsonElement("nombreCatalogo")]
        public string NombreCatalogo { get; set; } = null!;

        [BsonElement("descripcionCatalogo")]
        public string DescripcionCatalogo { get; set; } = null!;

        // Esta lista permite la acción de "meterle productos" al mismo componente
        [BsonElement("productos")]
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }
}
