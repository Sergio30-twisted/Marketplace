using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Catalogo.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Producto
    {
        [BsonElement("_id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Decimal128)]  // ← esto soluciona el $numberDecimal
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [BsonElement("stock")]
        [JsonPropertyName("stock")]
        public int Stock { get; set; }

        [BsonElement("isActive")]
        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}