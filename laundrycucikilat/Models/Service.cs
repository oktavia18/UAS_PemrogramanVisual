using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace laundrycucikilat.Models
{
    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("serviceId")]
        public string ServiceId { get; set; } = string.Empty;

        [BsonElement("namaLayanan")]
        public string NamaLayanan { get; set; } = string.Empty;

        [BsonElement("deskripsi")]
        public string Deskripsi { get; set; } = string.Empty;

        [BsonElement("hargaPerKg")]
        public decimal HargaPerKg { get; set; }

        [BsonElement("estimasiWaktu")]
        public string EstimasiWaktu { get; set; } = string.Empty;

        [BsonElement("kategori")]
        public string Kategori { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Aktif";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}