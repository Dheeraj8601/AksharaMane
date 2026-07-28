using AksharaMane.Application.Interfaces.Services;
using AksharaMane.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AksharaMane.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBookService,BookService>();
        services.AddScoped<IAdminAuthService,AdminAuthService>();
        services.AddScoped<IOrderService,OrderService>();
        services.AddScoped<IOrderEmailTemplateService,OrderEmailTemplateService>();
        services.AddScoped<IDashboardService,DashboardService>();

        return services;
    }
}