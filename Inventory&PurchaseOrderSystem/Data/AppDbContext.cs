using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ---- Member 1 scope ----
    public DbSet<User> Users => Set<User>();

    // ---- Reserved for Members 2 & 3 ----
    // Do NOT uncomment until they hand you their entity classes.
    // public DbSet<Product> Products => Set<Product>();
    // public DbSet<Supplier> Suppliers => Set<Supplier>();
    // public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    // public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}