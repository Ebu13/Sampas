using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using Northwind.Business.Request;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class ShipperService : IGenericService<ShipperRequestDTO>
    {
        private readonly NorthwindContext _context;

        public ShipperService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShipperRequestDTO>> GetAllAsync()
        {
            return await _context.Shippers
                .Select(s => new ShipperRequestDTO
                {
                    ShipperId = s.ShipperId,
                    ShipperName = s.ShipperName,
                    Phone = s.Phone
                })
                .ToListAsync();
        }

        public async Task<ShipperRequestDTO> GetByIdAsync(int id)
        {
            var shipper = await _context.Shippers
                .Where(s => s.ShipperId == id)
                .Select(s => new ShipperRequestDTO
                {
                    ShipperId = s.ShipperId,
                    ShipperName = s.ShipperName,
                    Phone = s.Phone
                })
                .FirstOrDefaultAsync();

            if (shipper == null)
            {
                throw new KeyNotFoundException("Shipper not found");
            }

            return shipper;
        }

        public async Task<ShipperRequestDTO> CreateAsync(ShipperRequestDTO shipperDto)
        {
            var shipper = new Shipper
            {
                ShipperName = shipperDto.ShipperName,
                Phone = shipperDto.Phone
            };

            _context.Shippers.Add(shipper);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            shipperDto.ShipperId = shipper.ShipperId;

            return shipperDto;
        }

        public async Task UpdateAsync(int id, ShipperRequestDTO shipperDto)
        {
            if (id != shipperDto.ShipperId)
            {
                throw new ArgumentException("Shipper ID mismatch");
            }

            var shipper = await _context.Shippers.FindAsync(id);

            if (shipper == null)
            {
                throw new KeyNotFoundException("Shipper not found");
            }

            shipper.ShipperName = shipperDto.ShipperName;
            shipper.Phone = shipperDto.Phone;

            _context.Entry(shipper).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shipper = await _context.Shippers.FindAsync(id);
            if (shipper == null)
            {
                throw new KeyNotFoundException("Shipper not found");
            }

            _context.Shippers.Remove(shipper);
            await _context.SaveChangesAsync();
        }
    }
}
