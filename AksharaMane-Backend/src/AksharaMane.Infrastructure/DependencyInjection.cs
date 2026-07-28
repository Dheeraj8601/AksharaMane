using AksharaMane.Application.Interfaces.Authentication;
using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Infrastructure.Authentication;
using AksharaMane.Infrastructure.Email;
using AksharaMane.Infrastructure.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AksharaMane.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(
                JwtSettings.SectionName));

        services.Configure<EmailSettings>(
            configuration.GetSection(
                EmailSettings.SectionName));

        services.AddScoped<
            IJwtTokenService,
            JwtTokenService>();

        services.AddScoped<
            IPasswordHasherService,
            PasswordHasherService>();

        services.AddScoped<
            IFileStorageService,
            LocalFileStorageService>();

        services.AddScoped<
            IEmailService,
            MailKitEmailService>();

        return services;
    }
}