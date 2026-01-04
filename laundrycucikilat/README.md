# Laundry Cuci Kilat - Website Laundry Modern dengan MongoDB

Website laundry dengan tampilan soft blue aesthetic yang modern dan responsif, dibangun menggunakan ASP.NET Core 8.0 dan MongoDB.

## 🌟 Fitur Utama

### 🏠 Halaman Home
- Hero section dengan banner menarik
- Tampilan fitur unggulan
- Preview layanan dengan harga
- Call-to-action yang jelas

### 🧺 Halaman Layanan
- **Cuci Kering** - Rp 5.000/kg (24 jam)
- **Cuci Setrika** - Rp 7.000/kg (48 jam)  
- **Dry Clean** - Rp 15.000/pcs (3-5 hari)
- **Express** - Rp 10.000/kg (6 jam)
- **Cuci Sepatu** - Rp 25.000/pasang (2-3 hari)
- **Cuci Tas & Koper** - Rp 35.000/pcs (3-5 hari)

### 📝 Sistem Pemesanan
- Form pemesanan yang user-friendly
- Kalkulasi harga otomatis
- Pilihan layanan tambahan (antar jemput, pewangi)
- Penjadwalan pengambilan
- Generate ID pesanan otomatis
- **✅ Terintegrasi dengan MongoDB**

### 📊 Status Pesanan
- Tracking pesanan real-time
- Progress tracker visual
- Pencarian berdasarkan ID pesanan
- Riwayat pesanan terbaru
- **✅ Data tersimpan di MongoDB**

### 💳 Sistem Pembayaran
- Multiple metode pembayaran:
  - Tunai (COD)
  - Transfer Bank (BCA, Mandiri, BNI, BRI)
  - E-Wallet (GoPay, OVO, DANA, ShopeePay)
  - QRIS
- Informasi rekening dan nomor e-wallet
- Konfirmasi pembayaran
- **✅ Update payment status ke MongoDB**

### 🧾 Struk & Invoice
- Generate invoice profesional
- Print dan download PDF
- Share via WhatsApp
- Detail lengkap pesanan dan pembayaran

### 📞 Halaman Kontak
- Informasi kontak lengkap
- Form kontak interaktif
- FAQ (Frequently Asked Questions)
- Jam operasional
- Quick contact via WhatsApp
- **✅ Pesan tersimpan di MongoDB**

## 🎨 Desain & UI/UX

### Tema Soft Blue Aesthetic
- **Primary Blue**: #87CEEB (Sky Blue)
- **Light Blue**: #E6F3FF
- **Soft Blue**: #B8E0FF
- **Dark Blue**: #4A90E2
- **Light Gray**: #F8F9FA
- **White**: #FFFFFF

### Fitur Desain
- ✅ Fully Responsive (Mobile, Tablet, Desktop)
- ✅ Modern Card-based Layout
- ✅ Smooth Animations & Transitions
- ✅ Font Awesome Icons
- ✅ Google Fonts (Poppins)
- ✅ Bootstrap 5 Framework
- ✅ Print-friendly Styles

## 🚀 Teknologi

- **Backend**: ASP.NET Core 8.0
- **Database**: MongoDB
- **Frontend**: HTML5, CSS3, JavaScript ES6
- **Framework**: Bootstrap 5.3
- **Icons**: Font Awesome 6.0
- **Fonts**: Google Fonts (Poppins)
- **API**: RESTful API dengan Controllers
- **Fallback**: LocalStorage untuk offline support

## 📱 Fitur Responsif

- **Mobile First Design**
- **Adaptive Navigation**
- **Touch-friendly Buttons**
- **Optimized Forms**
- **Responsive Tables**

## 🔧 Instalasi & Menjalankan

### Prerequisites
- .NET 8.0 SDK
- MongoDB (Local atau Atlas)
- Visual Studio 2022 atau VS Code

### Langkah Instalasi

1. **Clone Repository**
   ```bash
   git clone [repository-url]
   cd laundrycucikilat
   ```

2. **Setup MongoDB**
   - Install MongoDB Community Server atau gunakan MongoDB Atlas
   - Update connection string di `appsettings.json`
   - Lihat detail di `MONGODB_SETUP.md`

3. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

4. **Build Project**
   ```bash
   dotnet build
   ```

5. **Run Application**
   ```bash
   dotnet run
   ```

6. **Akses Website**
   ```
   http://localhost:5042
   ```

## 📂 Struktur Project

```
laundrycucikilat/
├── Controllers/
│   ├── OrderController.cs          # API untuk Orders
│   └── ContactController.cs        # API untuk Contact Messages
├── Models/
│   ├── Order.cs                    # Model Order
│   ├── ContactMessage.cs           # Model Contact Message
│   └── MongoDbSettings.cs          # MongoDB Configuration
├── Services/
│   └── MongoDbService.cs           # MongoDB Service Layer
├── Pages/
│   ├── Shared/
│   │   └── _Layout.cshtml          # Layout utama
│   ├── Index.cshtml                # Halaman Home
│   ├── Layanan.cshtml              # Halaman Layanan
│   ├── Pemesanan.cshtml            # Form Pemesanan
│   ├── Status.cshtml               # Status Pesanan
│   ├── Pembayaran.cshtml           # Sistem Pembayaran
│   ├── Struk.cshtml                # Invoice & Struk
│   └── Kontak.cshtml               # Halaman Kontak
├── wwwroot/
│   ├── css/
│   │   └── site.css                # Custom CSS
│   ├── js/
│   │   ├── mongodb-api.js          # MongoDB API Helper
│   │   └── site.js                 # Custom JavaScript
│   └── lib/                        # Libraries (Bootstrap, jQuery)
├── Program.cs                      # Entry point
├── appsettings.json               # Configuration
├── laundrycucikilat.csproj       # Project file
├── MONGODB_SETUP.md               # MongoDB Setup Guide
└── README.md                      # Documentation
```

## 💾 Database Schema

### Orders Collection
```json
{
  "_id": "ObjectId",
  "orderId": "LCK12345678",
  "namaLengkap": "John Doe",
  "noTelepon": "+62812345678",
  "alamat": "Jl. Contoh No. 123",
  "jenisLayanan": "cuci-setrika",
  "jumlah": 5,
  "antarJemput": true,
  "pewangiKhusus": false,
  "tanggalAmbil": "2024-01-15",
  "waktuAmbil": "10:00-12:00",
  "catatan": "Pisahkan baju putih",
  "total": "Rp 35.000",
  "status": "Menunggu Konfirmasi",
  "tanggalPesan": "15/01/2024",
  "estimasiSelesai": "17/01/2024",
  "paymentMethod": "transfer",
  "paymentStatus": "Lunas",
  "paymentDate": "15/01/2024",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

### ContactMessages Collection
```json
{
  "_id": "ObjectId",
  "nama": "Jane Doe",
  "email": "jane@example.com",
  "telepon": "+62812345678",
  "subjek": "informasi-layanan",
  "pesan": "Apakah ada diskon untuk pelanggan baru?",
  "tanggal": "15/01/2024",
  "waktu": "10:30:00",
  "isRead": false,
  "createdAt": "2024-01-15T10:30:00Z"
}
```

## 🔌 API Endpoints

### Orders API
- `GET /api/order` - Get all orders
- `GET /api/order/{orderId}` - Get specific order
- `GET /api/order/recent/{limit}` - Get recent orders
- `POST /api/order` - Create new order
- `PUT /api/order/{id}` - Update order
- `PATCH /api/order/{orderId}/status` - Update order status
- `PATCH /api/order/{orderId}/payment` - Update payment info
- `DELETE /api/order/{id}` - Delete order

### Contact API
- `GET /api/contact` - Get all messages
- `GET /api/contact/{id}` - Get specific message
- `GET /api/contact/unread-count` - Get unread count
- `POST /api/contact` - Create new message
- `PATCH /api/contact/{id}/mark-read` - Mark as read
- `DELETE /api/contact/{id}` - Delete message

## 🌐 Fitur Web Modern

### Progressive Web App Ready
- Responsive Design
- Fast Loading
- Offline Capability (dengan fallback ke localStorage)

### SEO Optimized
- Meta tags lengkap
- Semantic HTML
- Structured data ready

### Accessibility
- ARIA labels
- Keyboard navigation
- Screen reader friendly
- High contrast support

## 📋 Fitur Bisnis

### Manajemen Pesanan
- Generate ID pesanan unik
- Tracking status real-time
- Estimasi waktu selesai
- Update status otomatis

### Sistem Pembayaran
- Multiple payment methods
- Payment status tracking
- Invoice generation
- Receipt management

### Customer Service
- Contact form dengan database
- WhatsApp integration
- FAQ section
- Message management

## 🔒 Keamanan

- Input validation
- XSS protection
- CSRF protection
- Secure headers
- Data sanitization
- MongoDB injection prevention

## 📈 Performance

- Optimized database queries
- Connection pooling
- Async/await patterns
- Efficient data serialization
- Caching strategies
- CDN ready

## 🎯 Target Audience

- **Pelanggan Individu**: Layanan laundry personal
- **Keluarga**: Paket keluarga dengan diskon
- **Bisnis**: Layanan laundry untuk hotel, restoran
- **Mahasiswa**: Paket khusus mahasiswa

## 📞 Kontak & Support

- **Telepon**: +62 812-3456-7890
- **Email**: info@laundrycucikilat.com
- **WhatsApp**: +62 812-3456-7890
- **Alamat**: Jl. Contoh No. 123, Jakarta

## 📄 License

© 2024 Laundry Cuci Kilat. All rights reserved.

---

**✅ MONGODB INTEGRATION COMPLETED**
**🚀 Website berjalan di http://localhost:5042**
**📊 Data tersimpan di MongoDB dengan fallback localStorage**
**🔌 RESTful API tersedia untuk semua operasi**