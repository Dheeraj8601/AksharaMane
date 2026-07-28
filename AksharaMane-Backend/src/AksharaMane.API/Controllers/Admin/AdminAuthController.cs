using AksharaMane.Application.Common;
using AksharaMane.Application.DTOs.Auth;
using AksharaMane.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AksharaMane.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/auth")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IAdminAuthService _adminAuthService;

        public AdminAuthController(
            IAdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] AdminLoginRequestDto request,
            CancellationToken cancellationToken)
        {
            var result =
                await _adminAuthService.LoginAsync(
                    request,
                    cancellationToken);

            return Ok(
                ApiResponse<AdminLoginResponseDto>
                    .SuccessResponse(
                        result,
                        "Login successful."));
        }
    }
}
