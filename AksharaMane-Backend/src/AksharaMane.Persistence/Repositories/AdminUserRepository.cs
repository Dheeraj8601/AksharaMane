using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Domain.Entities;
using AksharaMane.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Persistence.Repositories
{
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly AksharaManeDbContext _context;

        public AdminUserRepository(
            AksharaManeDbContext context)
        {
            _context = context;
        }

        public async Task<AdminUser?> GetByEmailAsync(
            string email,
            bool trackChanges = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.AdminUsers.AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);
        }

        public async Task<bool> AnyAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.AdminUsers
                .AnyAsync(cancellationToken);
        }

        public async Task AddAsync(
            AdminUser adminUser,
            CancellationToken cancellationToken = default)
        {
            await _context.AdminUsers.AddAsync(
                adminUser,
                cancellationToken);
        }

        public async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}
