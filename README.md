# Project UAS Pemrograman Visual (Desktop)
## PROFIL
| Variable           |             Isi            |
| -------------------|----------------------------|
| **Nama**           |         Oktavia Rizkha Kurniawati       |
| **NIM**            |          312310509         |
| **Kelas**          |          TI.23.A.5         |
| **Mata Kuliah**    |     Pemrograman Visual (Desktop)     |
| **Dosen Pengampu** | Dr. Muhamad Fatchan, S.Kom., M.Kom. |

# UAS Pemrograman Visual – Laundry Cuci Kilat

## 📌 Deskripsi Proyek
**Laundry Cuci Kilat** adalah aplikasi berbasis web yang dibuat untuk memenuhi **Ujian Akhir Semester (UAS) mata kuliah Pemrograman Visual**.  
Aplikasi ini menampilkan sistem informasi layanan laundry dengan struktur web yang rapi dan mudah dikembangkan.

## 🎯 Tujuan dan Manfaat Sistem

### Tujuan
- Mengembangkan sistem informasi laundry berbasis web
- Menerapkan konsep pemrograman visual dalam aplikasi website
- Mengelola proses bisnis laundry secara terintegrasi
- Menyediakan sistem yang terstruktur dan mudah dikembangkan

### Manfaat
- Mempermudah pengelolaan data layanan dan pesanan laundry
- Meningkatkan efisiensi dan akurasi pencatatan
- Memudahkan pemantauan status pesanan
- Mendukung pelayanan laundry yang lebih profesional

---
## Cara Menggunakan Website Laundry Cuci Kilat

### 1. Menjalankan Aplikasi
- Buka project **Laundry Cuci Kilat** menggunakan **Visual Studio** atau **Visual Studio Code**
- Jalankan aplikasi melalui menu **Run** atau menggunakan perintah `dotnet run`
- Setelah aplikasi berhasil dijalankan, buka browser
- Akses website melalui alamat:
http://localhost:5042

yaml
Copy code

---

### 2. Login ke Sistem
- Akses halaman login melalui:
http://localhost:5042/Login

- Gunakan akun demo berikut untuk masuk ke sistem:

**Admin**
- Username: `admin@laundrycucikilat.com`
- Password: `admin123`

**Karyawan**
- Username: `karyawan@laundrycucikilat.com`
- Password: `karyawan123`

---

### 3. Menggunakan Fitur Sistem
- Pilih layanan laundry yang tersedia pada halaman layanan
- Klik tombol **Pesan Sekarang** untuk melakukan pemesanan
- Isi data pelanggan (nama, nomor telepon, dan alamat)
- Tentukan jenis layanan dan jumlah atau berat laundry
- Pilih layanan tambahan jika diperlukan (antar jemput, pewangi khusus)
- Konfirmasi dan kirim pesanan

---

### 4. Melihat Detail dan Status Pesanan
- Setelah pesanan berhasil dibuat, sistem akan menampilkan halaman detail pesanan
- Pengguna dapat melihat:
- ID pesanan
- Data pelanggan
- Jenis layanan
- Total biaya
- Status dan progres pengerjaan laundry
- Status pesanan dapat dipantau hingga proses selesai

---

### 5. Melihat, Mencetak, atau Mengunduh Struk
- Setelah pesanan diproses, struk pesanan dapat dilihat melalui menu detail pesanan
- Struk dapat:
- Dicetak langsung melalui browser
- Diunduh dalam format PDF
- Struk berisi informasi detail pesanan dan total pembayaran

---

### 6. Logout dari Sistem
- Klik nama pengguna pada navigation bar
- Pilih menu **Logout** untuk keluar dari sistem
- Pengguna akan diarahkan kembali ke halaman login

---

## 🛠️ Teknologi yang Digunakan

### Frontend
- **HTML5** – Struktur halaman website  
- **CSS3** – Desain dan tampilan antarmuka  
- **JavaScript** – Interaksi dan fungsi dinamis  
- **Bootstrap** – Framework CSS untuk tampilan responsif  

### Backend
- **PHP** – Bahasa pemrograman server-side  
- **Model-View-Controller (MVC)** – Arsitektur pemisahan logika, tampilan, dan data  

### Database
- **MySQL** – Sistem manajemen basis data untuk menyimpan data pengguna, layanan, pesanan, transaksi, dan laporan  

### Keamanan & Pendukung
- **Session Management** – Pengelolaan sesi login pengguna  
- **Password Encryption** – Keamanan data akun pengguna  
- **PDF Generator** – Pembuatan struk transaksi dan laporan  
- **Role-Based Access Control (RBAC)** – Pembagian hak akses pengguna  

### Version Control
- **Git & GitHub** – Pengelolaan source code dan dokumentasi project  

---

## ✨ Fitur Utama Sistem

- Sistem Login dan Autentikasi Pengguna
- Role-Based Access Control (Admin & Karyawan)
- Manajemen Layanan Laundry
- Pemesanan Laundry Online
- Manajemen dan Proses Pesanan
- Dashboard Operasional
- Manajemen Data Pelanggan
- Manajemen Data Karyawan
- Laporan Transaksi dan Pendapatan (PDF)
- Fitur Logout Aman


---

## 🎨 Tampilan / User Interface

Website dirancang dengan tampilan yang sederhana, modern, dan user-friendly, meliputi:
- Halaman Login
<img width="1919" height="1017" alt="Screenshot 2026-01-04 232202" src="https://github.com/user-attachments/assets/4e0cc5f8-3a85-4d1d-9f79-865dea979f8b" />

- Halaman Beranda
  <img width="1903" height="961" alt="Screenshot 2026-01-04 111615" src="https://github.com/user-attachments/assets/0ccf978a-16e7-4bb6-beb4-ba3269b241c7" />

- Halaman Layanan
  <img width="1899" height="1001" alt="Screenshot 2026-01-05 000729" src="https://github.com/user-attachments/assets/3c40497c-5807-4e28-8bcd-8912d112f1e3" />

- Dashboard Admin
  <img width="1919" height="879" alt="Screenshot 2026-01-04 112543" src="https://github.com/user-attachments/assets/36552a5a-3b41-461b-80f9-a30ac265b17f" />

- Dashboard Karyawan
  <img width="1899" height="1002" alt="Screenshot 2026-01-05 001218" src="https://github.com/user-attachments/assets/f7c01185-90e4-48d2-bc82-7aa1c1c830a4" />

- Halaman Kelola Data Karyawan
  <img width="1915" height="971" alt="Screenshot 2026-01-05 001558" src="https://github.com/user-attachments/assets/de34d8bb-82be-448b-9a47-8c868bb5dc09" />

- Detail Pesanan

<img width="652" height="771" alt="Screenshot 2026-01-04 112357" src="https://github.com/user-attachments/assets/c749fbee-aab4-4588-b592-75dc227d4dc4" />

- Struk Transaksi Laundry
  <img width="861" height="930" alt="Screenshot 2026-01-05 064043" src="https://github.com/user-attachments/assets/8e2d307b-3509-4a5c-bfd4-a389730e8caa" />

- Laporan Transaksi dalam format PDF
  <img width="1683" height="754" alt="Screenshot 2026-01-05 003522" src="https://github.com/user-attachments/assets/384fc027-6117-4e9d-bd53-5c063eb917a8" />


Tampilan dibuat responsif untuk mendukung penggunaan pada berbagai perangkat.

---

## 🎥 Demo Aplikasi (Video YouTube)

Untuk melihat secara langsung cara kerja dan fitur-fitur utama dari **Website Laundry Cuci Kilat**, silakan tonton video demo aplikasi melalui link YouTube berikut:

| Platform | Deskripsi | Link |
|--------|----------|------|
| YouTube | Demo Website Laundry Cuci Kilat – Penjelasan Fitur dan Alur Sistem | https://www.youtube.com/watch?v=LINK_VIDEO_ANDA |

> **Catatan:**  
> Video demo menampilkan proses penggunaan aplikasi mulai dari login, pemesanan laundry, pengelolaan pesanan oleh karyawan, hingga laporan transaksi oleh admin.

---

## 📌 Penutup
Website **Laundry Cuci Kilat** dikembangkan sebagai sistem informasi berbasis web yang bertujuan untuk meningkatkan efisiensi operasional bisnis laundry. Dengan adanya sistem ini, proses pemesanan, pengelolaan data, serta pelaporan transaksi dapat dilakukan secara terintegrasi, akurat, dan mudah digunakan oleh setiap peran pengguna.

Sistem ini diharapkan dapat menjadi solusi digital yang aplikatif serta menjadi referensi dalam pengembangan sistem informasi sejenis.

---

## 🙏 Terima Kasih
Terima kasih telah meluangkan waktu untuk melihat dan mencoba **Website Laundry Cuci Kilat**.  
Semoga aplikasi ini dapat memberikan manfaat serta wawasan dalam pengembangan sistem informasi berbasis web.

Jika terdapat saran, masukan, atau pertanyaan, silakan disampaikan melalui repository ini.
