using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;

namespace Northwind.Business.Services
{
    public class MessageService : IGenericService<MessageRequestDTO>
    {
        private readonly NorthwindContext _context;

        public MessageService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageRequestDTO>> GetAllAsync()
        {
            return await _context.Messages
                .Select(m => new MessageRequestDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    MessageText = m.MessageText,
                    SentDate = m.SentDate
                })
                .ToListAsync();
        }

        public async Task<MessageRequestDTO> GetByIdAsync(int id)
        {
            var message = await _context.Messages
                .Where(m => m.MessageId == id)
                .Select(m => new MessageRequestDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    MessageText = m.MessageText,
                    SentDate = m.SentDate
                })
                .FirstOrDefaultAsync();

            if (message == null)
            {
                throw new KeyNotFoundException("Message not found");
            }

            return message;
        }

        public async Task<MessageRequestDTO> CreateAsync(MessageRequestDTO messageDto)
        {
            var message = new Message
            {
                SenderId = messageDto.SenderId,
                ReceiverId = messageDto.ReceiverId,
                MessageText = messageDto.MessageText,
                SentDate = messageDto.SentDate ?? DateTime.UtcNow // Default to current time if not provided
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            messageDto.MessageId = message.MessageId;

            return messageDto;
        }

        public async Task UpdateAsync(int id, MessageRequestDTO messageDto)
        {
            if (id != messageDto.MessageId)
            {
                throw new ArgumentException("Message ID mismatch");
            }

            var message = await _context.Messages.FindAsync(id);

            if (message == null)
            {
                throw new KeyNotFoundException("Message not found");
            }

            message.SenderId = messageDto.SenderId;
            message.ReceiverId = messageDto.ReceiverId;
            message.MessageText = messageDto.MessageText;
            message.SentDate = messageDto.SentDate;

            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                throw new KeyNotFoundException("Message not found");
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
}
