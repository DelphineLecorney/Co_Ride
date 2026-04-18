using Shared.Contracts.DTOs.Identity;

namespace Identity.Application.Interfaces;

/// <summary>
/// Interface du service d'authentification
/// </summary>
public interface IAuthService
{
    Task<(bool Success, AuthResponse? Response, string? Error)> LoginAsync(LoginRequest request);
    Task<(bool Success, AuthResponse? Response, string? Error)> LogoutAsync(string refreshToken);
    Task<(bool Success, AuthResponse? Response, string? Error)> RegisterAsync(RegisterRequest request);
    Task<(bool Success, AuthResponse? Response, string? Error)> RefreshTokenAsync(string refreshToken);
    Task<(bool Success, string? Error)> LogoutAllDevicesAsync(Guid userId);
    Task<UserDto?> GetUserByIdAsync(Guid id);
}