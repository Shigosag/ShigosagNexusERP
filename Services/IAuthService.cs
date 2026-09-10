using System.Threading.Tasks;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public record AuthResult(bool IsSuccess, string? Token, User? User, string? ErrorMessage);

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password);
    void Logout();
    User? CurrentUser { get; }
    string? CurrentToken { get; }
    bool IsAuthenticated { get; }
}
