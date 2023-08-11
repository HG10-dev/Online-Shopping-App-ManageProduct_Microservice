using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ManageProduct_Microservice.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        //[JsonIgnore]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        [Required]
        public string Description { get; set; } = string.Empty;

        [BsonElement("isAvailable")]
        public bool Availability => Quantity != 0;

        [BsonElement("quantity")]
        [Required]
        public int Quantity { get; set; }

        [BsonElement("price")]
        [Required]
        public float Price { get ; set; }

        [BsonElement("category")]
        public string Category { get; set; } = "Product";
    }

    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonIgnore] public int Id { get; set; }

        [BsonElement("categoryName")]
        [Required]
        public string CategoryName { get; set; } = "Product";
    }
}
