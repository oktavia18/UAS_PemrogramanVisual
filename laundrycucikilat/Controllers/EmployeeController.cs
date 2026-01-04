using Microsoft.AspNetCore.Mvc;
using laundrycucikilat.Models;
using laundrycucikilat.Services;

namespace laundrycucikilat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;
        private readonly PdfService _pdfService;

        public EmployeeController(MongoDbService mongoDbService, PdfService pdfService)
        {
            _mongoDbService = mongoDbService;
            _pdfService = pdfService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var orders = await _mongoDbService.GetOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving orders", error = ex.Message });
            }
        }

        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var success = await _mongoDbService.UpdateOrderStatusAsync(orderId, request.Status);
                if (success)
                {
                    return Ok(new { message = "Status updated successfully" });
                }
                return NotFound(new { message = "Order not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating order status", error = ex.Message });
            }
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrderDetails(string orderId)
        {
            try
            {
                // Try to get order by orderId first
                var order = await _mongoDbService.GetOrderAsync(orderId);
                if (order != null)
                {
                    return Ok(order);
                }

                // If not found by orderId, try by MongoDB _id
                order = await _mongoDbService.GetOrderByIdAsync(orderId);
                if (order != null)
                {
                    return Ok(order);
                }

                return NotFound(new { message = "Order not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving order details", error = ex.Message });
            }
        }

        [HttpGet("customers/{customerId}")]
        public async Task<IActionResult> GetCustomerDetails(string customerId)
        {
            try
            {
                var customer = await _mongoDbService.GetCustomerByIdAsync(customerId);
                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound(new { message = "Customer not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving customer details", error = ex.Message });
            }
        }

        [HttpDelete("orders/{orderId}")]
        public async Task<IActionResult> DeleteOrder(string orderId)
        {
            try
            {
                var order = await _mongoDbService.GetOrderAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                if (!string.IsNullOrEmpty(order.Id))
                {
                    await _mongoDbService.RemoveOrderAsync(order.Id);
                    return Ok(new { message = "Order deleted successfully" });
                }
                
                return BadRequest(new { message = "Invalid order ID" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting order", error = ex.Message });
            }
        }

        [HttpGet("orders/{orderId}/pdf")]
        public async Task<IActionResult> GenerateOrderPdf(string orderId)
        {
            try
            {
                // Try to get order by orderId first
                var order = await _mongoDbService.GetOrderAsync(orderId);
                if (order == null)
                {
                    // If not found by orderId, try by MongoDB _id
                    order = await _mongoDbService.GetOrderByIdAsync(orderId);
                }

                if (order == null)
                {
                    return NotFound(new { message = "Order not found" });
                }

                // Generate PDF
                var pdfBytes = _pdfService.GenerateOrderDetailPdf(order);

                // Return PDF file
                var fileName = $"Detail_Pesanan_{order.OrderId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error generating PDF", error = ex.Message });
            }
        }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}