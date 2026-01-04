using Microsoft.AspNetCore.Mvc;
using laundrycucikilat.Models;
using laundrycucikilat.Services;

namespace laundrycucikilat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;
        private readonly PdfService _pdfService;

        public AdminController(MongoDbService mongoDbService, PdfService pdfService)
        {
            _mongoDbService = mongoDbService;
            _pdfService = pdfService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var orders = await _mongoDbService.GetOrdersAsync();
                var customers = await _mongoDbService.GetCustomersAsync();
                var employees = await _mongoDbService.GetEmployeesAsync();

                var dashboardData = new
                {
                    TotalOrders = orders.Count(),
                    TotalCustomers = customers.Count(),
                    TotalEmployees = employees.Count(),
                    TodayOrders = orders.Count(o => o.TanggalPesan == DateTime.Now.ToString("yyyy-MM-dd")),
                    PendingOrders = orders.Count(o => o.Status == "Menunggu Konfirmasi"),
                    CompletedOrders = orders.Count(o => o.Status == "Selesai"),
                    TotalRevenue = orders.Where(o => o.Status == "Selesai").Sum(o => decimal.Parse(o.Total.Replace("Rp ", "").Replace(".", ""))),
                    RecentOrders = orders.OrderByDescending(o => o.CreatedAt).Take(5)
                };

                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving dashboard data", error = ex.Message });
            }
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var services = await _mongoDbService.GetServicesAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving services", error = ex.Message });
            }
        }

        [HttpPost("services")]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            try
            {
                service.ServiceId = Guid.NewGuid().ToString("N")[..8].ToUpper();
                await _mongoDbService.CreateServiceAsync(service);
                return Ok(new { message = "Service created successfully", serviceId = service.ServiceId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating service", error = ex.Message });
            }
        }

        [HttpPut("services/{serviceId}")]
        public async Task<IActionResult> UpdateService(string serviceId, [FromBody] Service service)
        {
            try
            {
                var success = await _mongoDbService.UpdateServiceAsync(serviceId, service);
                if (success)
                {
                    return Ok(new { message = "Service updated successfully" });
                }
                return NotFound(new { message = "Service not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating service", error = ex.Message });
            }
        }

        [HttpDelete("services/{serviceId}")]
        public async Task<IActionResult> DeleteService(string serviceId)
        {
            try
            {
                var success = await _mongoDbService.DeleteServiceAsync(serviceId);
                if (success)
                {
                    return Ok(new { message = "Service deleted successfully" });
                }
                return NotFound(new { message = "Service not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting service", error = ex.Message });
            }
        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _mongoDbService.GetCustomersAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving customers", error = ex.Message });
            }
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _mongoDbService.GetEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving employees", error = ex.Message });
            }
        }

        [HttpPost("employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                employee.EmployeeId = "EMP" + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 9999);
                await _mongoDbService.CreateEmployeeAsync(employee);
                return Ok(new { message = "Employee created successfully", employeeId = employee.EmployeeId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating employee", error = ex.Message });
            }
        }

        [HttpGet("employees/{employeeId}")]
        public async Task<IActionResult> GetEmployee(string employeeId)
        {
            try
            {
                var employee = await _mongoDbService.GetEmployeeAsync(employeeId);
                if (employee != null)
                {
                    return Ok(employee);
                }
                return NotFound(new { message = "Employee not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving employee", error = ex.Message });
            }
        }

        [HttpPut("employees/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(string employeeId, [FromBody] Employee employee)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(employee.NamaLengkap) || 
                    string.IsNullOrEmpty(employee.Email) || 
                    string.IsNullOrEmpty(employee.NoTelepon) || 
                    string.IsNullOrEmpty(employee.Jabatan))
                {
                    return BadRequest(new { message = "Field wajib tidak boleh kosong" });
                }

                // Validate phone number (only digits, spaces, hyphens, and plus allowed)
                if (!System.Text.RegularExpressions.Regex.IsMatch(employee.NoTelepon, @"^[\d\s\-\+\(\)]+$"))
                {
                    return BadRequest(new { message = "Nomor telepon hanya boleh berisi angka" });
                }

                // Validate email format
                if (!System.Text.RegularExpressions.Regex.IsMatch(employee.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return BadRequest(new { message = "Format email tidak valid" });
                }

                // Ensure employeeId cannot be changed
                employee.EmployeeId = employeeId;
                
                var success = await _mongoDbService.UpdateEmployeeAsync(employeeId, employee);
                if (success)
                {
                    return Ok(new { message = "Data karyawan berhasil diperbarui" });
                }
                return NotFound(new { message = "Karyawan tidak ditemukan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating employee", error = ex.Message });
            }
        }

        [HttpDelete("employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            try
            {
                // Check if employee exists
                var employee = await _mongoDbService.GetEmployeeAsync(employeeId);
                if (employee == null)
                {
                    return NotFound(new { message = "Karyawan tidak ditemukan" });
                }

                // Perform hard delete (permanent deletion from database)
                var success = await _mongoDbService.DeleteEmployeeAsync(employeeId);
                if (success)
                {
                    return Ok(new { message = "Karyawan berhasil dihapus permanen", hardDelete = true });
                }
                
                return StatusCode(500, new { message = "Gagal menghapus karyawan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting employee", error = ex.Message });
            }
        }

        [HttpDelete("employees/{employeeId}/permanent")]
        public async Task<IActionResult> PermanentDeleteEmployee(string employeeId)
        {
            try
            {
                var success = await _mongoDbService.DeleteEmployeeAsync(employeeId);
                if (success)
                {
                    return Ok(new { message = "Karyawan berhasil dihapus permanen" });
                }
                return NotFound(new { message = "Karyawan tidak ditemukan" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error permanently deleting employee", error = ex.Message });
            }
        }

        [HttpGet("reports/transactions")]
        public async Task<IActionResult> GetTransactionReport([FromQuery] string? startDate, [FromQuery] string? endDate)
        {
            try
            {
                var orders = await _mongoDbService.GetOrdersAsync();
                
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    try
                    {
                        var startDateTime = DateTime.Parse(startDate);
                        var endDateTime = DateTime.Parse(endDate);
                        
                        orders = orders.Where(o => 
                        {
                            // Try multiple date parsing approaches
                            DateTime orderDate;
                            
                            // First try direct parsing
                            if (DateTime.TryParse(o.TanggalPesan, out orderDate))
                            {
                                return orderDate.Date >= startDateTime.Date && orderDate.Date <= endDateTime.Date;
                            }
                            
                            // Try parsing with specific formats
                            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "dd-MM-yyyy" };
                            foreach (var format in formats)
                            {
                                if (DateTime.TryParseExact(o.TanggalPesan, format, null, System.Globalization.DateTimeStyles.None, out orderDate))
                                {
                                    return orderDate.Date >= startDateTime.Date && orderDate.Date <= endDateTime.Date;
                                }
                            }
                            
                            return false;
                        }).ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Date filtering error in GetTransactionReport: {ex.Message}");
                        // If date parsing fails, use all orders
                    }
                }

                // Calculate revenue with improved parsing
                decimal totalRevenue = 0;
                foreach (var order in orders.Where(o => o.Status == "Selesai"))
                {
                    if (!string.IsNullOrEmpty(order.Total))
                    {
                        var cleanTotal = order.Total
                            .Replace("Rp", "")
                            .Replace(".", "")
                            .Replace(",", "")
                            .Replace(" ", "")
                            .Trim();
                        
                        if (decimal.TryParse(cleanTotal, out decimal amount))
                        {
                            totalRevenue += amount;
                        }
                    }
                }

                var report = new
                {
                    TotalTransactions = orders.Count(),
                    TotalRevenue = totalRevenue,
                    CompletedOrders = orders.Count(o => o.Status == "Selesai"),
                    PendingOrders = orders.Count(o => o.Status != "Selesai"),
                    Transactions = orders.OrderByDescending(o => o.CreatedAt)
                };

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating transaction report", error = ex.Message });
            }
        }

        [HttpGet("reports/transactions/export-pdf")]
        public async Task<IActionResult> ExportTransactionReportToPdf([FromQuery] string? startDate, [FromQuery] string? endDate)
        {
            try
            {
                // Debug logging
                Console.WriteLine($"Export PDF called with startDate: {startDate}, endDate: {endDate}");
                
                var allOrders = await _mongoDbService.GetOrdersAsync();
                Console.WriteLine($"Total orders from database: {allOrders.Count}");

                // Filter orders by date range if provided
                var filteredOrders = allOrders.ToList();
                
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    try
                    {
                        var startDateTime = DateTime.Parse(startDate);
                        var endDateTime = DateTime.Parse(endDate);
                        
                        Console.WriteLine($"Filtering from {startDateTime:yyyy-MM-dd} to {endDateTime:yyyy-MM-dd}");
                        
                        filteredOrders = allOrders.Where(o => 
                        {
                            // Try multiple date parsing approaches
                            DateTime orderDate;
                            
                            // First try direct parsing
                            if (DateTime.TryParse(o.TanggalPesan, out orderDate))
                            {
                                var isInRange = orderDate.Date >= startDateTime.Date && orderDate.Date <= endDateTime.Date;
                                if (isInRange)
                                {
                                    Console.WriteLine($"Order {o.OrderId} date {orderDate:yyyy-MM-dd} is in range");
                                }
                                return isInRange;
                            }
                            
                            // Try parsing with specific formats
                            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "dd-MM-yyyy" };
                            foreach (var format in formats)
                            {
                                if (DateTime.TryParseExact(o.TanggalPesan, format, null, System.Globalization.DateTimeStyles.None, out orderDate))
                                {
                                    var isInRange = orderDate.Date >= startDateTime.Date && orderDate.Date <= endDateTime.Date;
                                    if (isInRange)
                                    {
                                        Console.WriteLine($"Order {o.OrderId} date {orderDate:yyyy-MM-dd} (format: {format}) is in range");
                                    }
                                    return isInRange;
                                }
                            }
                            
                            Console.WriteLine($"Could not parse date for order {o.OrderId}: '{o.TanggalPesan}'");
                            return false;
                        }).ToList();
                        
                        Console.WriteLine($"Filtered orders count: {filteredOrders.Count}");
                        
                        // Log some sample filtered orders
                        foreach (var order in filteredOrders.Take(3))
                        {
                            Console.WriteLine($"Sample filtered order: {order.OrderId}, Date: {order.TanggalPesan}, Status: {order.Status}, Total: {order.Total}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Date parsing error: {ex.Message}");
                        // If date parsing fails, use all orders
                        filteredOrders = allOrders.ToList();
                    }
                }

                // Log final statistics before PDF generation
                var completedCount = filteredOrders.Count(o => o.Status == "Selesai");
                var pendingCount = filteredOrders.Count(o => o.Status != "Selesai");
                Console.WriteLine($"Final stats before PDF: Total={filteredOrders.Count}, Completed={completedCount}, Pending={pendingCount}");

                // Generate PDF with filtered data
                var pdfBytes = _pdfService.GenerateTransactionReportPdf(filteredOrders, startDate ?? "N/A", endDate ?? "N/A");

                // Create filename
                var fileName = $"Laporan_Transaksi_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    try
                    {
                        var startFormatted = DateTime.Parse(startDate).ToString("yyyyMMdd");
                        var endFormatted = DateTime.Parse(endDate).ToString("yyyyMMdd");
                        fileName = $"Laporan_Transaksi_{startFormatted}_{endFormatted}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    }
                    catch
                    {
                        // Use default filename if date parsing fails
                    }
                }

                Console.WriteLine($"Generated PDF with {pdfBytes.Length} bytes, filename: {fileName}");

                // Return PDF file with proper headers
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportTransactionReportToPdf: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Error generating PDF report", error = ex.Message });
            }
        }

        [HttpGet("reports/all-transactions/export-pdf")]
        public async Task<IActionResult> ExportAllTransactionReportToPdf()
        {
            try
            {
                // Get all orders without date filtering
                var orders = await _mongoDbService.GetOrdersAsync();

                // Generate PDF for all data
                var pdfBytes = _pdfService.GenerateAllTransactionReportPdf(orders);

                // Create filename
                var fileName = $"laporan-transaksi-semua-data_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                // Return PDF file with proper headers
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating complete PDF report", error = ex.Message });
            }
        }
    }
}