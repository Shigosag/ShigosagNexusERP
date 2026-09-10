using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IJwtTokenService _jwtService;

    public User? CurrentUser { get; private set; }
    public string? CurrentToken { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(CurrentToken) && _jwtService.IsTokenValid(CurrentToken);

    public AuthService(AppDbContext db, IJwtTokenService jwtService)
    {
        _db = db;
        _jwtService = jwtService;
    }

    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        try
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            if (user == null)
            {
                return new AuthResult(false, null, null, "Invalid credentials provided.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return new AuthResult(false, null, null, "Invalid credentials provided.");
            }

            user.LastLoginAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);
            CurrentUser = user;
            CurrentToken = token;

            return new AuthResult(true, token, user, null);
        }
        catch (Exception ex)
        {
            return new AuthResult(false, null, null, $"Authentication subsystem failure: {ex.Message}");
        }
    }

    public void Logout()
    {
        CurrentUser = null;
        CurrentToken = null;
    }
}
