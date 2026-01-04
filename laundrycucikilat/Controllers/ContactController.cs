using Microsoft.AspNetCore.Mvc;
using laundrycucikilat.Models;
using laundrycucikilat.Services;

namespace laundrycucikilat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public ContactController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactMessage>>> GetContactMessages()
        {
            var messages = await _mongoDbService.GetContactMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactMessage>> GetContactMessage(string id)
        {
            var message = await _mongoDbService.GetContactMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return Ok(message);
        }

        [HttpGet("unread-count")]
        public async Task<ActionResult<long>> GetUnreadMessagesCount()
        {
            var count = await _mongoDbService.GetUnreadMessagesCountAsync();
            return Ok(new { count });
        }

        [HttpPost]
        public async Task<ActionResult<ContactMessage>> CreateContactMessage(ContactMessage message)
        {
            message.Tanggal = DateTime.Now.ToString("dd/MM/yyyy");
            message.Waktu = DateTime.Now.ToString("HH:mm:ss");
            
            await _mongoDbService.CreateContactMessageAsync(message);
            return CreatedAtAction(nameof(GetContactMessage), new { id = message.Id }, message);
        }

        [HttpPatch("{id}/mark-read")]
        public async Task<IActionResult> MarkMessageAsRead(string id)
        {
            var message = await _mongoDbService.GetContactMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            await _mongoDbService.MarkMessageAsReadAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactMessage(string id)
        {
            var message = await _mongoDbService.GetContactMessageAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            await _mongoDbService.RemoveContactMessageAsync(id);
            return NoContent();
        }
    }
}