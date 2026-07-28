using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.DTOs.Dashboard
{
    public class AdminDashboardResponseDto
    {
        public int TotalBooks { get; set; }

        public int ActiveBooks { get; set; }

        public int TotalCategories { get; set; }

        public int ActiveCategories { get; set; }

        public int TotalOrders { get; set; }

        public int PlacedOrders { get; set; }

        public int ConfirmedOrders { get; set; }

        public int ShippedOrders { get; set; }

        public int DeliveredOrders { get; set; }

        public int CancelledOrders { get; set; }

        public decimal TotalRevenue { get; set; }

        public List<LowStockBookDto> LowStockBooks { get; set; } = [];

        public List<RecentOrderDto> RecentOrders { get; set; } = [];

        public decimal TodayRevenue { get; set; }

        public decimal ThisMonthRevenue { get; set; }

        public int TodayOrders { get; set; }

        public int LowStockCount { get; set; }

        public int OutOfStockCount { get; set; }
    }
}
