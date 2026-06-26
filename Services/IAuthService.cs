using System.Threading.Tasks;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public interface IAuthService
{
    Task<User?> LoginAsync(string username, string password);
}