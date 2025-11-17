using Nps.Application.Interfaces.Persistence;
using Nps.Application.Interfaces.Security;
using Nps.Domain.Entities;
using Nps.Domain.Enums;

namespace Nps.Infrastructure.Seed;

public class DatabaseSeeder
{
    public static async Task SeedAdminUserAsync(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    {
        var existingAdmin = await userRepository.GetAnyAdminAsync();
        if (existingAdmin is not null)
            return;

        var admin = new User(
            username: "admin",
            passwordHash: passwordHasher.Hash("Admin123!"),
            role: UserRole.Admin
        );

        await userRepository.AddAsync(admin);
    }
}