using InventoryApi.Models;
using Microsoft.AspNetCore.Identity;

namespace Inventory_PurchaseOrderSystem.Services
{
    public class UserService
    {
        private readonly PasswordHasher<User> _hasher = new();

        public async Task RegisterAsync(RegisterDto request)
        {
            var user = new User();
            user.PasswordHash = _hasher.HashPassword(user, request.Password);
        }

        public async Task<User> ValidateAsync(LoginDto request)
        {
            var user = new User();
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new AppException("Invalid username or password.", 401);
            }
            return user;
        }
    }
}