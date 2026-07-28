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
    public class OrderRepository : IOrderRepository
    {
        private readonly AksharaManeDbContext _context;

        public OrderRepository(
            AksharaManeDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> GetQueryable()
        {
            return _context.Orders
                .Include(x => x.Book)
                .AsQueryable();
        }

        public async Task<Order?> GetByIdAsync(
            int id,
            bool trackChanges = false,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Orders
                .Include(x => x.Book)
                .AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
        }

        public async Task<Order?> GetByOrderNumberAsync(
            string orderNumber,
            CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.Book)
                .FirstOrDefaultAsync(
                    x => x.OrderNumber == orderNumber,
                    cancellationToken);
        }

        public async Task AddAsync(
            Order order,
            CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(
                order,
                cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<Order?> GetByOrderNumberForUpdateAsync(string orderNumber,CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(x => x.Book)
                .FirstOrDefaultAsync(
                    x => x.OrderNumber == orderNumber,
                    cancellationToken);
        }
    }
}
