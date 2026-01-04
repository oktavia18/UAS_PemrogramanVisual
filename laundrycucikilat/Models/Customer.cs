using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace laundrycucikilat.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("namaLengkap")]
        public string NamaLengkap { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("noTelepon")]
        public string NoTelepon { get; set; } = string.Empty;

        [BsonElement("alamat")]
        public string Alamat { get; set; } = string.Empty;

        [BsonElement("tanggalDaftar")]
        public string TanggalDaftar { get; set; } = string.Empty;

        [BsonElement("totalPesanan")]
        public int TotalPesanan { get; set; } = 0;

        [BsonElement("totalBelanja")]
        public decimal TotalBelanja { get; set; } = 0;

        [BsonElement("status")]
        public string Status { get; set; } = "Aktif";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}