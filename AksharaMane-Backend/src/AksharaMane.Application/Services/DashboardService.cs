using AksharaMane.Application.DTOs.Dashboard;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private const int LowStockThreshold = 5;
        private const int RecentOrderCount = 5;

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(
            IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<AdminDashboardResponseDto> GetAsync(
            CancellationToken cancellationToken = default)
        {
            var data =
                await _dashboardRepository.GetDashboardAsync(
                    LowStockThreshold,
                    RecentOrderCount,
                    cancellationToken);

            return new AdminDashboardResponseDto
            {
                TotalBooks = data.TotalBooks,
                ActiveBooks = data.ActiveBooks,
                TotalCategories = data.TotalCategories,
                ActiveCategories = data.ActiveCategories,
                TotalOrders = data.TotalOrders,
                PlacedOrders = data.PlacedOrders,
                ConfirmedOrders = data.ConfirmedOrders,
                ShippedOrders = data.ShippedOrders,
                DeliveredOrders = data.DeliveredOrders,
                CancelledOrders = data.CancelledOrders,
                TotalRevenue = data.TotalRevenue,
                LowStockBooks = data.LowStockBooks,
                RecentOrders = data.RecentOrders,
                TodayRevenue = data.TodayRevenue,
                ThisMonthRevenue = data.ThisMonthRevenue,
                TodayOrders = data.TodayOrders,
                LowStockCount = data.LowStockCount,
                OutOfStockCount = data.OutOfStockCount,
            };
        }
    }
}
