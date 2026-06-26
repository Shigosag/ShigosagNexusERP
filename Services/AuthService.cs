using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public class AuthService(AppDbContext db) : IAuthService
{
    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return user;
        }
        return null;
    }
}