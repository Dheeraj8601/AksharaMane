using AksharaMane.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Interfaces.Services
{
    public interface IAdminAuthService
    {
        Task<AdminLoginResponseDto> LoginAsync(AdminLoginRequestDto request,CancellationToken cancellationToken = default);
    }
}
