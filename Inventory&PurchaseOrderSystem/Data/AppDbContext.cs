using Inventory_PurchaseOrderSystem.Models;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderLine> PurchaseOrderLines => Set<PurchaseOrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.Property(u => u.Username).HasMaxLength(50).IsRequired();
            e.Property(u => u.FullName).HasMaxLength(100).IsRequired();
            e.Property(u => u.Role).HasMaxLength(20).IsRequired();
            e.HasIndex(u => u.Username).IsUnique();
            e.ToTable(t => t.HasCheckConstraint("CK_Users_Role", "[Role] IN (N'Admin', N'Staff')"));
        });

        modelBuilder.Entity<Supplier>(e =>
        {
            e.Property(s => s.Name).HasMaxLength(100).IsRequired();
            e.Property(s => s.ContactEmail).HasMaxLength(100).IsRequired();
            e.Property(s => s.Phone).HasMaxLength(30).IsRequired();
            e.Property(s => s.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Sku).HasMaxLength(30).IsRequired();
            e.Property(p => p.Name).HasMaxLength(100).IsRequired();
            e.Property(p => p.UnitPrice).HasColumnType("decimal(18,2)");
            e.Property(p => p.QuantityOnHand).HasDefaultValue(0);
            e.Property(p => p.ReorderLevel).HasDefaultValue(0);
            e.Property(p => p.IsActive).HasDefaultValue(true);
            e.HasIndex(p => p.Sku).IsUnique();
            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Products_QuantityOnHand", "QuantityOnHand >= 0");
                t.HasCheckConstraint("CK_Products_UnitPrice", "UnitPrice > 0");
                t.HasCheckConstraint("CK_Products_ReorderLevel", "ReorderLevel >= 0");
            });
        });

        modelBuilder.Entity<PurchaseOrder>(e =>
        {
            e.Property(p => p.PoNumber).HasMaxLength(30).IsRequired();
            e.Property(p => p.Status).HasMaxLength(20).IsRequired().HasDefaultValue(PurchaseOrderStatus.Draft);
            e.Property(p => p.Notes).HasMaxLength(500);
            e.Property(p => p.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            e.HasIndex(p => p.PoNumber).IsUnique();
            e.HasIndex(p => p.SupplierId);
            e.HasIndex(p => p.CreatedByUserId);
            e.HasIndex(p => p.Status);

            e.HasOne(p => p.Supplier).WithMany(s => s.PurchaseOrders)
             .HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.CreatedByUser).WithMany()
             .HasForeignKey(p => p.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);

            e.ToTable(t => t.HasCheckConstraint("CK_PO_Status",
                "[Status] IN (N'Draft', N'Submitted', N'Received', N'Cancelled')"));
        });

        modelBuilder.Entity<PurchaseOrderLine>(e =>
        {
            e.Property(l => l.UnitCost).HasColumnType("decimal(18,2)");
            e.HasIndex(l => l.PurchaseOrderId);
            e.HasIndex(l => l.ProductId);

            e.HasOne(l => l.PurchaseOrder).WithMany(p => p.Lines)
             .HasForeignKey(l => l.PurchaseOrderId).OnDelete(DeleteBehavior.Cascade);

            e.HasOne(l => l.Product).WithMany(p => p.PurchaseOrderLines)
             .HasForeignKey(l => l.ProductId).OnDelete(DeleteBehavior.Restrict);

            e.ToTable(t =>
            {
                t.HasCheckConstraint("CK_POL_Quantity", "Quantity > 0");
                t.HasCheckConstraint("CK_POL_UnitCost", "UnitCost > 0");
            });
        });
    }
}