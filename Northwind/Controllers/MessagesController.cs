using Microsoft.AspNetCore.Mvc;
using Northwind.Business.Services;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessageDetailService _messageDetailService;

        public MessagesController(MessageDetailService messageDetailService)
        {
            _messageDetailService = messageDetailService;
        }

        // GET: api/Messages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDetailRequestDTO>>> GetMessages()
        {
            var messages = await _messageDetailService.GetAllAsync();
            return Ok(messages);
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDetailRequestDTO>> GetMessage(int id)
        {
            var message = await _messageDetailService.GetByIdAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<ActionResult<MessageDetailRequestDTO>> PostMessage(MessageDetailRequestDTO messageDto)
        {
            var createdMessage = await _messageDetailService.CreateAsync(messageDto);
            return CreatedAtAction(nameof(GetMessage), new { id = createdMessage.MessageId }, createdMessage);
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, MessageDetailRequestDTO messageDto)
        {
            if (id != messageDto.MessageId)
            {
                return BadRequest("Message ID mismatch");
            }

            try
            {
                await _messageDetailService.UpdateAsync(id, messageDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _messageDetailService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
