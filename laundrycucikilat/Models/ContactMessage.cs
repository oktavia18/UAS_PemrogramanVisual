using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace laundrycucikilat.Models
{
    public class ContactMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nama")]
        public string Nama { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("telepon")]
        public string? Telepon { get; set; }

        [BsonElement("subjek")]
        public string Subjek { get; set; } = string.Empty;

        [BsonElement("pesan")]
        public string Pesan { get; set; } = string.Empty;

        [BsonElement("tanggal")]
        public string Tanggal { get; set; } = string.Empty;

        [BsonElement("waktu")]
        public string Waktu { get; set; } = string.Empty;

        [BsonElement("isRead")]
        public bool IsRead { get; set; } = false;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}