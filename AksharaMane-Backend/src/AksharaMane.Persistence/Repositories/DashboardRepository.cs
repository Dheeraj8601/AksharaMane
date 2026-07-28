using AksharaMane.Application.Common.Models;
using AksharaMane.Application.DTOs.Dashboard;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Domain.Enums;
using AksharaMane.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Persistence.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AksharaManeDbContext _context;

        public DashboardRepository(
            AksharaManeDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardData> GetDashboardAsync(
            int lowStockThreshold,
            int recentOrderCount,
            CancellationToken cancellationToken = default)
        {
            var totalBooks = await _context.Books
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var activeBooks = await _context.Books
                .AsNoTracking()
                .CountAsync(
                    x => x.IsActive,
                    cancellationToken);

            var totalCategories = await _context.Categories
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var activeCategories = await _context.Categories
                .AsNoTracking()
                .CountAsync(
                    x => x.IsActive,
                    cancellationToken);

            var totalOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(cancellationToken);

            var placedOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.Status == OrderStatus.Placed,
                    cancellationToken);

            var confirmedOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.Status == OrderStatus.Confirmed,
                    cancellationToken);

            var shippedOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.Status == OrderStatus.Shipped,
                    cancellationToken);

            var deliveredOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.Status == OrderStatus.Delivered,
                    cancellationToken);

            var cancelledOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.Status == OrderStatus.Cancelled,
                    cancellationToken);

            var totalRevenue = await _context.Orders
                .AsNoTracking()
                .Where(x => x.Status == OrderStatus.Delivered)
                .SumAsync(
                    x => (decimal?)x.TotalAmount,
                    cancellationToken) ?? 0;

            var lowStockBooks = await _context.Books
                .AsNoTracking()
                .Where(x =>
                    x.IsActive &&
                    x.StockQuantity <= lowStockThreshold)
                .OrderBy(x => x.StockQuantity)
                .ThenBy(x => x.Title)
                .Take(10)
                .Select(x => new LowStockBookDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    StockQuantity = x.StockQuantity,
                    ImageUrl = x.ImageUrl
                })
                .ToListAsync(cancellationToken);

            var recentOrders = await _context.Orders
                .AsNoTracking()
                .Include(x => x.Book)
                .OrderByDescending(x => x.CreatedAt)
                .Take(recentOrderCount)
                .Select(x => new RecentOrderDto
                {
                    Id = x.Id,
                    OrderNumber = x.OrderNumber,
                    CustomerName = x.CustomerName,
                    BookTitle = x.Book.Title,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var monthStart = new DateTime(
                DateTime.UtcNow.Year,
                DateTime.UtcNow.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc);

            var todayOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(
                    x => x.CreatedAt >= today &&
                         x.CreatedAt < tomorrow,
                    cancellationToken);

            var todayRevenue = await _context.Orders
                .AsNoTracking()
                .Where(x =>
                    x.Status == OrderStatus.Delivered &&
                    x.CreatedAt >= today &&
                    x.CreatedAt < tomorrow)
                .SumAsync(
                    x => (decimal?)x.TotalAmount,
                    cancellationToken) ?? 0;

            var thisMonthRevenue = await _context.Orders
                .AsNoTracking()
                .Where(x =>
                    x.Status == OrderStatus.Delivered &&
                    x.CreatedAt >= monthStart)
                .SumAsync(
                    x => (decimal?)x.TotalAmount,
                    cancellationToken) ?? 0;

            var lowStockCount = await _context.Books
                .AsNoTracking()
                .CountAsync(
                    x => x.IsActive &&
                         x.StockQuantity > 0 &&
                         x.StockQuantity <= lowStockThreshold,
                    cancellationToken);

            var outOfStockCount = await _context.Books
                .AsNoTracking()
                .CountAsync(
                    x => x.IsActive &&
                         x.StockQuantity == 0,
                    cancellationToken);

            return new AdminDashboardData
            {
                TotalBooks = totalBooks,
                ActiveBooks = activeBooks,
                TotalCategories = totalCategories,
                ActiveCategories = activeCategories,
                TotalOrders = totalOrders,
                PlacedOrders = placedOrders,
                ConfirmedOrders = confirmedOrders,
                ShippedOrders = shippedOrders,
                DeliveredOrders = deliveredOrders,
                CancelledOrders = cancelledOrders,
                TotalRevenue = totalRevenue,
                LowStockBooks = lowStockBooks,
                RecentOrders = recentOrders,
                TodayRevenue = todayRevenue,
                ThisMonthRevenue = thisMonthRevenue,
                TodayOrders = todayOrders,
                LowStockCount = lowStockCount,
                OutOfStockCount = outOfStockCount,
            };
        }
    }
}
