using AksharaMane.Application.Interfaces.Repositories;
using AksharaMane.Persistence.DbContexts;
using AksharaMane.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AksharaMane.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection is missing.");

        services.AddDbContext<AksharaManeDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IAdminUserRepository,AdminUserRepository>();
        services.AddScoped<IOrderRepository,OrderRepository>();
        services.AddScoped<IDashboardRepository,DashboardRepository>();

        return services;
    }
}