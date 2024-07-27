using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IGenericService<MessageRequestDTO> _messageService;

        public MessageController(IGenericService<MessageRequestDTO> messageService)
        {
            _messageService = messageService;
        }

        // GET: api/message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageRequestDTO>>> GetMessages()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        // GET: api/message/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageRequestDTO>> GetMessage(int id)
        {
            try
            {
                var message = await _messageService.GetByIdAsync(id);
                return Ok(message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/message
        [HttpPost]
        public async Task<ActionResult<MessageRequestDTO>> PostMessage(MessageRequestDTO messageDto)
        {
            var createdMessage = await _messageService.CreateAsync(messageDto);
            return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.MessageId }, createdMessage);
        }

        // PUT: api/message/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, MessageRequestDTO messageDto)
        {
            if (id != messageDto.MessageId)
            {
                return BadRequest("Message ID mismatch");
            }

            try
            {
                await _messageService.UpdateAsync(id, messageDto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _messageService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
