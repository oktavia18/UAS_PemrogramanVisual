using Microsoft.AspNetCore.Mvc;
using laundrycucikilat.Models;
using laundrycucikilat.Services;

namespace laundrycucikilat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;
        private readonly PdfService _pdfService;

        public OrderController(MongoDbService mongoDbService, PdfService pdfService)
        {
            _mongoDbService = mongoDbService;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _mongoDbService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(string orderId)
        {
            var order = await _mongoDbService.GetOrderAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("recent/{limit:int?}")]
        public async Task<ActionResult<List<Order>>> GetRecentOrders(int limit = 10)
        {
            var orders = await _mongoDbService.GetRecentOrdersAsync(limit);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {
            // Generate unique order ID
            order.OrderId = "LCK" + DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            order.TanggalPesan = DateTime.Now.ToString("dd/MM/yyyy");
            
            await _mongoDbService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrder), new { orderId = order.OrderId }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, Order order)
        {
            var existingOrder = await _mongoDbService.GetOrderByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            order.Id = id;
            await _mongoDbService.UpdateOrderAsync(id, order);
            return NoContent();
        }

        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, [FromBody] string status)
        {
            var order = await _mongoDbService.GetOrderAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            await _mongoDbService.UpdateOrderStatusAsync(orderId, status);
            return NoContent();
        }

        [HttpPatch("{orderId}/payment")]
        public async Task<IActionResult> UpdateOrderPayment(string orderId, [FromBody] PaymentUpdateRequest request)
        {
            var order = await _mongoDbService.GetOrderAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            await _mongoDbService.UpdateOrderPaymentAsync(orderId, request.PaymentMethod, request.PaymentStatus, request.PaymentDate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _mongoDbService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _mongoDbService.RemoveOrderAsync(id);
            return NoContent();
        }

        [HttpGet("{orderId}/pdf")]
        public async Task<IActionResult> GenerateOrderPdf(string orderId)
        {
            try
            {
                // Try to get order by orderId first
                var order = await _mongoDbService.GetOrderAsync(orderId);
                if (order == null)
                {
                    return NotFound(new { message = "Pesanan tidak ditemukan" });
                }

                // Generate PDF
                var pdfBytes = _pdfService.GenerateOrderDetailPdf(order);

                // Return PDF file
                var fileName = $"Detail_Pesanan_{order.OrderId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Terjadi kesalahan saat membuat PDF", error = ex.Message });
            }
        }
    }

    public class PaymentUpdateRequest
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentDate { get; set; } = string.Empty;
    }
}