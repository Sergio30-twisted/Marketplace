using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ReviewService.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        public string ProductId { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [MinLength(10, ErrorMessage = "El comentario debe tener al menos 10 caracteres.")]
        [MaxLength(1000, ErrorMessage = "El comentario no puede exceder 1000 caracteres.")]
        public string Comment { get; set; } = null!;

        [Range(1, 5, ErrorMessage = "El rating debe ser entre 1 y 5.")]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}