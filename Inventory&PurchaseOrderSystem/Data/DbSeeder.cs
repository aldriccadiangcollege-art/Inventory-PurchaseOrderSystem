using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Users.AnyAsync()) return;

        db.Users.AddRange(
            new User
            {
                Username = "admin",
                Email = "admin@inventory.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = Roles.Admin
            },
            new User
            {
                Username = "staff",
                Email = "staff@inventory.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
                Role = Roles.Staff
            }
        );

        await db.SaveChangesAsync();
    }
}