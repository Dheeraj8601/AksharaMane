using AksharaMane.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetQueryable();

        Task<Order?> GetByIdAsync(int id,bool trackChanges = false,CancellationToken cancellationToken = default);

        Task<Order?> GetByOrderNumberAsync( string orderNumber, CancellationToken cancellationToken = default);

        Task AddAsync(Order order,CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<Order?> GetByOrderNumberForUpdateAsync( string orderNumber, CancellationToken cancellationToken = default);
    }
}
