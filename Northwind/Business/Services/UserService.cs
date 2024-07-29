﻿using global::Northwind.Data;
using global::Northwind.Models;
using Microsoft.EntityFrameworkCore;
using Northwind.Data;
using Northwind.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

    namespace Northwind.Business.Services
    {
        public class UserService : IGenericService<User>
        {
            private readonly NorthwindContext _context;

            public UserService(NorthwindContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<User>> GetAllAsync()
            {
                return await _context.Users.ToListAsync();
            }

            public async Task<User> GetByIdAsync(int id)
            {
                return await _context.Users.FindAsync(id);
            }

            public async Task<User> CreateAsync(User entity)
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }

            public async Task UpdateAsync(int id, User entity)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    user.Username = entity.Username;
                    user.Password = entity.Password;
                    user.Role = entity.Role;
                    user.PersonId = entity.PersonId; // Eğer PersonId alanı kullanılıyorsa
                    await _context.SaveChangesAsync();
                }
            }

            public async Task DeleteAsync(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }

    }
}

