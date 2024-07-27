using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class MessageDetailService : IGenericService<MessageDetailRequestDTO>
    {
        private readonly NorthwindContext _context;

        public MessageDetailService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MessageDetailRequestDTO>> GetAllAsync()
        {
            return await _context.Messages
                .Select(m => new MessageDetailRequestDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    MessageText = m.MessageText,
                    SentDate = m.SentDate,
                    ReceiverFirstName_LastName = _context.Employees
                        .Where(e => e.EmployeeId == m.ReceiverId)
                        .Select(e => e.FirstName + " " + e.LastName)
                        .FirstOrDefault(),
                    SenderFirstName_LastName = _context.Employees
                        .Where(e => e.EmployeeId == m.SenderId)
                        .Select(e => e.FirstName + " " + e.LastName)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<MessageDetailRequestDTO> GetByIdAsync(int id)
        {
            var message = await _context.Messages
                .Where(m => m.MessageId == id)
                .Select(m => new MessageDetailRequestDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    MessageText = m.MessageText,
                    SentDate = m.SentDate,
                    ReceiverFirstName_LastName = _context.Employees
                        .Where(e => e.EmployeeId == m.ReceiverId)
                        .Select(e => e.FirstName + " " + e.LastName)
                        .FirstOrDefault(),
                    SenderFirstName_LastName = _context.Employees
                        .Where(e => e.EmployeeId == m.SenderId)
                        .Select(e => e.FirstName + " " + e.LastName)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();

            if (message == null)
            {
                throw new KeyNotFoundException("Message not found");
            }

            return message;
        }

        public async Task<MessageDetailRequestDTO> CreateAsync(MessageDetailRequestDTO messageDto)
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

            messageDto.ReceiverFirstName_LastName = _context.Employees
                .Where(e => e.EmployeeId == message.ReceiverId)
                .Select(e => e.FirstName + " " + e.LastName)
                .FirstOrDefault();
            messageDto.SenderFirstName_LastName = _context.Employees
                .Where(e => e.EmployeeId == message.SenderId)
                .Select(e => e.FirstName + " " + e.LastName)
                .FirstOrDefault();

            return messageDto;
        }

        public async Task UpdateAsync(int id, MessageDetailRequestDTO messageDto)
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
