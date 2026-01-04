# MongoDB Setup untuk Laundry Cuci Kilat

## 📋 Prerequisites

### 1. Install MongoDB
Pilih salah satu cara berikut:

#### Option A: MongoDB Community Server (Recommended)
1. Download dari: https://www.mongodb.com/try/download/community
2. Install sesuai OS Anda
3. Start MongoDB service

#### Option B: MongoDB Atlas (Cloud)
1. Daftar di: https://www.mongodb.com/atlas
2. Buat cluster gratis
3. Dapatkan connection string

#### Option C: Docker (Quick Setup)
```bash
docker run --name mongodb -p 27017:27017 -d mongo:latest
```

### 2. Install MongoDB Tools (Optional)
- MongoDB Compass (GUI): https://www.mongodb.com/products/compass
- MongoDB Shell: https://www.mongodb.com/try/download/shell

## 🔧 Konfigurasi

### 1. Update Connection String
Edit file `appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",  // Local MongoDB
    // Atau untuk MongoDB Atlas:
    // "ConnectionString": "mongodb+srv://username:password@cluster.mongodb.net/",
    "DatabaseName": "LaundryDB",
    "OrdersCollectionName": "Orders",
    "ContactMessagesCollectionName": "ContactMessages"
  }
}
```

### 2. Untuk MongoDB Atlas
Jika menggunakan MongoDB Atlas, update connection string:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://<username>:<password>@<cluster-url>/<database>?retryWrites=true&w=majority",
    "DatabaseName": "LaundryDB",
    "OrdersCollectionName": "Orders",
    "ContactMessagesCollectionName": "ContactMessages"
  }
}
```

## 🚀 Menjalankan Aplikasi

### 1. Restore NuGet Packages
```bash
dotnet restore
```

### 2. Build Project
```bash
dotnet build
```

### 3. Run Application
```bash
dotnet run
```

### 4. Akses Website
```
http://localhost:5041
```

## 📊 Database Structure

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

## 🛠️ Testing API

### Using curl:
```bash
# Create Order
curl -X POST http://localhost:5041/api/order \
  -H "Content-Type: application/json" \
  -d '{
    "namaLengkap": "Test User",
    "noTelepon": "+62812345678",
    "alamat": "Test Address",
    "jenisLayanan": "cuci-setrika",
    "jumlah": 3,
    "antarJemput": true,
    "pewangiKhusus": false,
    "tanggalAmbil": "2024-01-20",
    "waktuAmbil": "10:00-12:00",
    "catatan": "Test order",
    "total": "Rp 21.000",
    "estimasiSelesai": "22/01/2024"
  }'

# Get Order
curl http://localhost:5041/api/order/LCK12345678
```

### Using Browser:
- Orders: http://localhost:5041/api/order
- Recent Orders: http://localhost:5041/api/order/recent/5
- Contact Messages: http://localhost:5041/api/contact

## 🔍 Monitoring

### MongoDB Compass
1. Connect to: `mongodb://localhost:27017`
2. Browse database: `LaundryDB`
3. View collections: `Orders`, `ContactMessages`

### Logs
Check console output untuk connection status dan errors.

## 🚨 Troubleshooting

### Connection Issues
1. **MongoDB not running**: Start MongoDB service
2. **Wrong connection string**: Check appsettings.json
3. **Network issues**: Check firewall/antivirus
4. **Atlas issues**: Check IP whitelist dan credentials

### Common Errors
- `MongoConnectionException`: MongoDB tidak berjalan
- `MongoAuthenticationException`: Credentials salah
- `TimeoutException`: Network atau firewall issue

### Fallback Mode
Jika MongoDB tidak tersedia, aplikasi akan fallback ke localStorage untuk demo purposes.

## 📈 Production Considerations

### Security
- Use authentication (username/password)
- Enable SSL/TLS
- Configure firewall rules
- Use connection pooling

### Performance
- Create indexes untuk query yang sering digunakan
- Monitor memory usage
- Setup replica sets untuk high availability

### Backup
- Setup automated backups
- Test restore procedures
- Monitor disk space

## 🔗 Useful Links

- [MongoDB Documentation](https://docs.mongodb.com/)
- [MongoDB .NET Driver](https://mongodb.github.io/mongo-csharp-driver/)
- [MongoDB Atlas](https://www.mongodb.com/atlas)
- [MongoDB Compass](https://www.mongodb.com/products/compass)