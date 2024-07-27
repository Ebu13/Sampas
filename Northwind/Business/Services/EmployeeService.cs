using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Business.Request;
using Northwind.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Business.Services
{
    public class EmployeeService : IGenericService<EmployeeRequestDTO>
    {
        private readonly NorthwindContext _context;

        public EmployeeService(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeRequestDTO>> GetAllAsync()
        {
            return await _context.Employees
                .Select(e => new EmployeeRequestDTO
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Notes = e.Notes,
                    Password = e.Password
                })
                .ToListAsync();
        }

        public async Task<EmployeeRequestDTO> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Where(e => e.EmployeeId == id)
                .Select(e => new EmployeeRequestDTO
                {
                    EmployeeId = e.EmployeeId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Notes = e.Notes,
                    Password = e.Password
                })
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            return employee;
        }

        public async Task<EmployeeRequestDTO> CreateAsync(EmployeeRequestDTO employeeDto)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                BirthDate = employeeDto.BirthDate,
                Notes = employeeDto.Notes,
                Password = employeeDto.Password
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // DTO'ya ID'yi ekleyin
            employeeDto.EmployeeId = employee.EmployeeId;

            return employeeDto;
        }

        public async Task UpdateAsync(int id, EmployeeRequestDTO employeeDto)
        {
            if (id != employeeDto.EmployeeId)
            {
                throw new ArgumentException("Employee ID mismatch");
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.BirthDate = employeeDto.BirthDate;
            employee.Notes = employeeDto.Notes;
            employee.Password = employeeDto.Password;

            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}
