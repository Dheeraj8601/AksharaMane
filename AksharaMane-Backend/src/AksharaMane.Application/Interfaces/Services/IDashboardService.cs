using AksharaMane.Application.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<AdminDashboardResponseDto> GetAsync(CancellationToken cancellationToken = default);
    }
}
