using MongoDB.Driver;
using laundrycucikilat.Models;
using System.Security.Cryptography;
using System.Text;

namespace laundrycucikilat.Services
{
    public class AuthService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<Employee> _employees;

        public AuthService(MongoDbService mongoDbService)
        {
            _users = mongoDbService.GetCollection<User>("Users");
            _employees = mongoDbService.GetCollection<Employee>("Employees");
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Find user by email or username
                var filter = Builders<User>.Filter.Or(
                    Builders<User>.Filter.Eq(u => u.Email, request.EmailOrUsername.ToLower()),
                    Builders<User>.Filter.Eq(u => u.Username, request.EmailOrUsername.ToLower())
                );

                var user = await _users.Find(filter).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email/Username atau password salah"
                    };
                }

                if (!user.IsActive)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Akun Anda tidak aktif. Hubungi administrator."
                    };
                }

                // Verify password
                if (!VerifyPassword(request.Password, user.Password))
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Email/Username atau password salah"
                    };
                }

                // Update last login
                var update = Builders<User>.Update.Set(u => u.LastLoginAt, DateTime.UtcNow);
                await _users.UpdateOneAsync(u => u.Id == user.Id, update);

                // Determine redirect URL based on role
                string redirectUrl = user.Role switch
                {
                    UserRole.Admin => "/Admin",
                    UserRole.Employee => "/Karyawan",
                    _ => "/" // Default to homepage
                };

                return new LoginResponse
                {
                    Success = true,
                    Message = "Login berhasil",
                    User = user,
                    RedirectUrl = redirectUrl
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Terjadi kesalahan sistem. Silakan coba lagi."
                };
            }
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                // Check if email or username already exists
                var existingUser = await _users.Find(u => 
                    u.Email == user.Email.ToLower() || 
                    u.Username == user.Username.ToLower()).FirstOrDefaultAsync();

                if (existingUser != null)
                {
                    return false;
                }

                // Hash password
                user.Password = HashPassword(user.Password);
                user.Email = user.Email.ToLower();
                user.Username = user.Username.ToLower();
                user.CreatedAt = DateTime.UtcNow;

                await _users.InsertOneAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            try
            {
                var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }

                // Verify current password
                if (!VerifyPassword(request.CurrentPassword, user.Password))
                {
                    return false;
                }

                // Update password
                var hashedNewPassword = HashPassword(request.NewPassword);
                var update = Builders<User>.Update.Set(u => u.Password, hashedNewPassword);
                await _users.UpdateOneAsync(u => u.Id == userId, update);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _users.Find(_ => true).ToListAsync();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<bool> DeactivateUserAsync(string userId)
        {
            try
            {
                var update = Builders<User>.Update.Set(u => u.IsActive, false);
                var result = await _users.UpdateOneAsync(u => u.Id == userId, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActivateUserAsync(string userId)
        {
            try
            {
                var update = Builders<User>.Update.Set(u => u.IsActive, true);
                var result = await _users.UpdateOneAsync(u => u.Id == userId, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "LaundrySecretSalt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }

        public async Task SeedDefaultUsersAsync()
        {
            try
            {
                // Check if users already exist
                var userCount = await _users.CountDocumentsAsync(_ => true);
                if (userCount > 0)
                {
                    return; // Users already exist
                }

                // Create default admin user
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@laundrycucikilat.com",
                    Password = "admin123", // Will be hashed
                    FullName = "Administrator",
                    Role = UserRole.Admin,
                    IsActive = true
                };

                // Create default employee user
                var employeeUser = new User
                {
                    Username = "karyawan",
                    Email = "karyawan@laundrycucikilat.com",
                    Password = "karyawan123", // Will be hashed
                    FullName = "Karyawan Laundry",
                    Role = UserRole.Employee,
                    IsActive = true
                };

                await CreateUserAsync(adminUser);
                await CreateUserAsync(employeeUser);
            }
            catch (Exception ex)
            {
                // Log error if needed
                Console.WriteLine($"Error seeding users: {ex.Message}");
            }
        }
    }
}