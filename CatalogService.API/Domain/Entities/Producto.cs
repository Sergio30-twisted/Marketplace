using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace Catalogo.API.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Producto
    {
        [BsonId]
        [BsonSerializer(typeof(Int32Serializer))]
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } 

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

    }
}
