using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Dashboard;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AksharaMane.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardController(
            IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard(
            CancellationToken cancellationToken)
        {
            var result =
                await _dashboardService.GetAsync(
                    cancellationToken);

            return Ok(
                ApiResponse<AdminDashboardResponseDto>
                    .SuccessResponse(result));
        }
    }
}
