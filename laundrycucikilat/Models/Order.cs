using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace laundrycucikilat.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("orderId")]
        public string OrderId { get; set; } = string.Empty;

        [BsonElement("namaLengkap")]
        public string NamaLengkap { get; set; } = string.Empty;

        [BsonElement("noTelepon")]
        public string NoTelepon { get; set; } = string.Empty;

        [BsonElement("alamat")]
        public string Alamat { get; set; } = string.Empty;

        [BsonElement("jenisLayanan")]
        public string JenisLayanan { get; set; } = string.Empty;

        [BsonElement("jumlah")]
        public int Jumlah { get; set; }

        [BsonElement("antarJemput")]
        public bool AntarJemput { get; set; }

        [BsonElement("pewangiKhusus")]
        public bool PewangiKhusus { get; set; }

        [BsonElement("tanggalAmbil")]
        public string TanggalAmbil { get; set; } = string.Empty;

        [BsonElement("waktuAmbil")]
        public string WaktuAmbil { get; set; } = string.Empty;

        [BsonElement("catatan")]
        public string? Catatan { get; set; }

        [BsonElement("total")]
        public string Total { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Menunggu Konfirmasi";

        [BsonElement("tanggalPesan")]
        public string TanggalPesan { get; set; } = string.Empty;

        [BsonElement("estimasiSelesai")]
        public string EstimasiSelesai { get; set; } = string.Empty;

        [BsonElement("paymentMethod")]
        public string? PaymentMethod { get; set; }

        [BsonElement("paymentStatus")]
        public string? PaymentStatus { get; set; }

        [BsonElement("paymentDate")]
        public string? PaymentDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}