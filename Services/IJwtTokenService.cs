using System.Security.Claims;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    ClaimsPrincipal? ValidateToken(string token);
    bool IsTokenValid(string token);
}
