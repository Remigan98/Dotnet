using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext context;
        private readonly IPasswordHasher<User> passwordHasher;

        public AccountService(RestaurantDbContext context, IPasswordHasher<User> passwordHasher)
        {
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        public async Task RegisterUserAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
        {
            // Optional pre-check (still enforce unique index in DB)
            bool emailExists = await context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == dto.Email, cancellationToken);

            if (emailExists)
            {
                // You can throw a custom exception handled by your ErrorHandlingMiddleware
                throw new InvalidOperationException("Email is already in use.");
            }

            User newUser = new User
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, dto.Password);

            await context.Users.AddAsync(newUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
