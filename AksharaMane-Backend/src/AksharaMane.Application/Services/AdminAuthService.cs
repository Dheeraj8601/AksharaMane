using AksharaMane.Application.Common.Exceptions;
using AksharaMane.Application.DTOs.Auth;
using AksharaMane.Application.Interfaces.Authentication;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Application.Services
{
    public class AdminAuthService : IAdminAuthService
    {
        private readonly IAdminUserRepository _adminUserRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IJwtTokenService _jwtTokenService;

        public AdminAuthService(
            IAdminUserRepository adminUserRepository,
            IPasswordHasherService passwordHasherService,
            IJwtTokenService jwtTokenService)
        {
            _adminUserRepository = adminUserRepository;
            _passwordHasherService = passwordHasherService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AdminLoginResponseDto> LoginAsync(
            AdminLoginRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var normalizedEmail = request.Email
                .Trim()
                .ToLowerInvariant();

            var admin = await _adminUserRepository.GetByEmailAsync(
                normalizedEmail,
                cancellationToken: cancellationToken);

            if (admin is null)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }

            if (!admin.IsActive)
            {
                throw new UnauthorizedException(
                    "Your admin account is inactive.");
            }

            var isPasswordValid =
                _passwordHasherService.VerifyPassword(
                    request.Password,
                    admin.PasswordHash);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException(
                    "Invalid email or password.");
            }

            var tokenResult =
                _jwtTokenService.GenerateToken(admin);

            return new AdminLoginResponseDto
            {
                AdminId = admin.Id,
                Name = admin.Name,
                Email = admin.Email,
                Role = admin.Role,
                AccessToken = tokenResult.AccessToken,
                ExpiresAt = tokenResult.ExpiresAt
            };
        }
    }
}
