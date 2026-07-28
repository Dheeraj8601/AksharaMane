using AksharaMane.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AksharaMane.Persistence.DbContexts;

public class AksharaManeDbContext : DbContext
{
    public AksharaManeDbContext(
        DbContextOptions<AksharaManeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AksharaManeDbContext).Assembly);
    }
}