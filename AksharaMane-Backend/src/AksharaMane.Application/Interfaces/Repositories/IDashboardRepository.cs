using AksharaMane.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<AdminDashboardData> GetDashboardAsync(int lowStockThreshold,int recentOrderCount,CancellationToken cancellationToken = default);
    }
}
