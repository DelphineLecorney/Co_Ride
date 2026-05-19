using Shared.Contracts.DTOs.Identity;

namespace Blazor.Client.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(LoginRequest form);
        Task Logout();
        Task<Guid> GetCurrentUserIdAsync();
    }
}
