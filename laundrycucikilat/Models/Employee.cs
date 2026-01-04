using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace laundrycucikilat.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [BsonElement("namaLengkap")]
        public string NamaLengkap { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("noTelepon")]
        public string NoTelepon { get; set; } = string.Empty;

        [BsonElement("jabatan")]
        public string Jabatan { get; set; } = string.Empty;

        [BsonElement("alamat")]
        public string Alamat { get; set; } = string.Empty;

        [BsonElement("tanggalMasuk")]
        public string TanggalMasuk { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Aktif";

        [BsonElement("gaji")]
        public decimal Gaji { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}