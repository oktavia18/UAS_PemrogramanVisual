using laundrycucikilat.Models;

namespace laundrycucikilat.Services
{
    public class DataSeeder
    {
        private readonly MongoDbService _mongoDbService;

        public DataSeeder(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task SeedDataAsync()
        {
            await SeedServicesAsync();
            await SeedEmployeesAsync();
            await SeedCustomersAsync();
            await SeedSampleOrdersAsync();
        }

        private async Task SeedServicesAsync()
        {
            var existingServices = await _mongoDbService.GetServicesAsync();
            if (existingServices.Any()) return;

            var services = new List<Service>
            {
                new Service
                {
                    ServiceId = "SVC001",
                    NamaLayanan = "Cuci Kering",
                    Deskripsi = "Layanan cuci kering standar dengan deterjen berkualitas",
                    HargaPerKg = 5000,
                    EstimasiWaktu = "2-3 hari",
                    Kategori = "Cuci Kering",
                    Status = "Aktif"
                },
                new Service
                {
                    ServiceId = "SVC002",
                    NamaLayanan = "Cuci Setrika",
                    Deskripsi = "Layanan cuci lengkap dengan setrika rapi",
                    HargaPerKg = 7000,
                    EstimasiWaktu = "3-4 hari",
                    Kategori = "Cuci Setrika",
                    Status = "Aktif"
                },
                new Service
                {
                    ServiceId = "SVC003",
                    NamaLayanan = "Dry Clean",
                    Deskripsi = "Layanan dry clean untuk pakaian khusus",
                    HargaPerKg = 15000,
                    EstimasiWaktu = "4-5 hari",
                    Kategori = "Dry Clean",
                    Status = "Aktif"
                },
                new Service
                {
                    ServiceId = "SVC004",
                    NamaLayanan = "Express 24 Jam",
                    Deskripsi = "Layanan cuci setrika express dalam 24 jam",
                    HargaPerKg = 12000,
                    EstimasiWaktu = "1 hari",
                    Kategori = "Express",
                    Status = "Aktif"
                }
            };

            foreach (var service in services)
            {
                await _mongoDbService.CreateServiceAsync(service);
            }
        }

        private async Task SeedEmployeesAsync()
        {
            var existingEmployees = await _mongoDbService.GetEmployeesAsync();
            if (existingEmployees.Any()) return;

            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = "EMP001",
                    NamaLengkap = "Siti Nurhaliza",
                    Email = "siti@laundrycucikilat.com",
                    NoTelepon = "081234567890",
                    Jabatan = "Manager",
                    Alamat = "Jl. Sudirman No. 123, Jakarta",
                    TanggalMasuk = "2023-01-15",
                    Status = "Aktif",
                    Gaji = 8000000
                },
                new Employee
                {
                    EmployeeId = "EMP002",
                    NamaLengkap = "Budi Santoso",
                    Email = "budi@laundrycucikilat.com",
                    NoTelepon = "081234567891",
                    Jabatan = "Supervisor",
                    Alamat = "Jl. Thamrin No. 456, Jakarta",
                    TanggalMasuk = "2023-02-01",
                    Status = "Aktif",
                    Gaji = 6000000
                },
                new Employee
                {
                    EmployeeId = "EMP003",
                    NamaLengkap = "Dewi Sartika",
                    Email = "dewi@laundrycucikilat.com",
                    NoTelepon = "081234567892",
                    Jabatan = "Operator Cuci",
                    Alamat = "Jl. Gatot Subroto No. 789, Jakarta",
                    TanggalMasuk = "2023-03-10",
                    Status = "Aktif",
                    Gaji = 4500000
                },
                new Employee
                {
                    EmployeeId = "EMP004",
                    NamaLengkap = "Ahmad Fauzi",
                    Email = "ahmad@laundrycucikilat.com",
                    NoTelepon = "081234567893",
                    Jabatan = "Customer Service",
                    Alamat = "Jl. Kuningan No. 321, Jakarta",
                    TanggalMasuk = "2023-04-05",
                    Status = "Aktif",
                    Gaji = 4000000
                }
            };

            foreach (var employee in employees)
            {
                await _mongoDbService.CreateEmployeeAsync(employee);
            }
        }

        private async Task SeedCustomersAsync()
        {
            var existingCustomers = await _mongoDbService.GetCustomersAsync();
            if (existingCustomers.Any()) return;

            var customers = new List<Customer>
            {
                new Customer
                {
                    CustomerId = "CUST001",
                    NamaLengkap = "Andi Wijaya",
                    Email = "andi.wijaya@email.com",
                    NoTelepon = "081234567894",
                    Alamat = "Jl. Merdeka No. 100, Jakarta",
                    TanggalDaftar = "2023-01-20",
                    TotalPesanan = 5,
                    TotalBelanja = 150000,
                    Status = "Aktif"
                },
                new Customer
                {
                    CustomerId = "CUST002",
                    NamaLengkap = "Sari Indah",
                    Email = "sari.indah@email.com",
                    NoTelepon = "081234567895",
                    Alamat = "Jl. Kemerdekaan No. 200, Jakarta",
                    TanggalDaftar = "2023-02-15",
                    TotalPesanan = 3,
                    TotalBelanja = 90000,
                    Status = "Aktif"
                },
                new Customer
                {
                    CustomerId = "CUST003",
                    NamaLengkap = "Rudi Hartono",
                    Email = "rudi.hartono@email.com",
                    NoTelepon = "081234567896",
                    Alamat = "Jl. Proklamasi No. 300, Jakarta",
                    TanggalDaftar = "2023-03-01",
                    TotalPesanan = 8,
                    TotalBelanja = 240000,
                    Status = "Aktif"
                }
            };

            foreach (var customer in customers)
            {
                await _mongoDbService.CreateCustomerAsync(customer);
            }
        }

        private async Task SeedSampleOrdersAsync()
        {
            var existingOrders = await _mongoDbService.GetOrdersAsync();
            if (existingOrders.Any()) return;

            var sampleOrders = new List<Order>
            {
                new Order
                {
                    OrderId = "LCK768847879",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Setrika",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "Mohon dicuci dengan hati-hati",
                    Total = "Rp 14.000",
                    Status = "Diterima",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768547840",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Kering",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 14.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Transfer",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768848834",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Setrika",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 18.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768848153",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Setrika",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 18.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768851350",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Setrika",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 18.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768851899",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Cuci Setrika",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 18.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-02",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768852304",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Dry Clean",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-02",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 70.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-05",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                },
                new Order
                {
                    OrderId = "LCK768853443",
                    NamaLengkap = "Oktaviani",
                    NoTelepon = "87657858",
                    Alamat = "Jl. Sudirman No. 123, Jakarta Pusat",
                    JenisLayanan = "Express",
                    Jumlah = 2,
                    AntarJemput = false,
                    PewangiKhusus = false,
                    TanggalAmbil = "2024-01-01",
                    WaktuAmbil = "10:00",
                    Catatan = "",
                    Total = "Rp 24.000",
                    Status = "Menunggu Konfirmasi",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd"),
                    EstimasiSelesai = "2024-01-01",
                    PaymentMethod = "Cash",
                    PaymentStatus = "Belum Lunas"
                }
            };

            foreach (var order in sampleOrders)
            {
                await _mongoDbService.CreateOrderAsync(order);
            }
        }
    }
}