using AksharaMane.Application.Interfaces.Authentication;
using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AksharaMane.Persistence.Seed
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider serviceProvider)
        {
            using var scope =
                serviceProvider.CreateScope();

            var repository =
                scope.ServiceProvider
                    .GetRequiredService<
                        IAdminUserRepository>();

            var passwordHasher =
                scope.ServiceProvider
                    .GetRequiredService<
                        IPasswordHasherService>();

            if (await repository.AnyAsync())
            {
                return;
            }

            var admin = new AdminUser
            {
                Name = "AksharaMane Admin",
                Email = "admin@aksharamane.com",
                PasswordHash =
                    passwordHasher.HashPassword(
                        "Admin@123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await repository.AddAsync(admin);

            await repository.SaveChangesAsync();
        }
    }
}
