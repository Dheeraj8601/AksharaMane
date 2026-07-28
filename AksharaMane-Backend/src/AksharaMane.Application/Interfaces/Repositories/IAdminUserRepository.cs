using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Repositories
{
    public interface IAdminUserRepository
    {
        Task<AdminUser?> GetByEmailAsync(
            string email,
            bool trackChanges = false,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            CancellationToken cancellationToken = default);

        Task AddAsync(
            AdminUser adminUser,
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
