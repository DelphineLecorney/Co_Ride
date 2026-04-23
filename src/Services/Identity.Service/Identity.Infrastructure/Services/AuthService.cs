using AutoMapper;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTOs.Identity;
using Shared.Messaging.SharedMessagingDomainEvents;

namespace Identity.Infrastructure.Services;

/// <summary>
/// Service d'authentification
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IMapper _mapper;


    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IPublishEndpoint publishEndpoint,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger,
        IMapper mapper
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _publishEndpoint = publishEndpoint;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    private string? GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }

    private string? GetDeviceInfo()
    {
        return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
    }

    public async Task<(bool Success, AuthResponse? Response, string? Error)> LoginAsync(LoginRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return (false, null, "L'email et le mot de passe sont requis");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("L'utilisateur avec l'email {Email} est introuvable", request.Email);
                return (false, null, "Email ou mot de passe invalide");
            }

            if (user.IsDeleted)
            {
                _logger.LogWarning("Tentative de connexion sur un compte supprimé : {UserId}", user.Id);
                return (false, null, "Ce compte a été supprimé");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true
            );

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Compte verrouillé : {UserId}", user.Id);
                    return (false, null, "Le compte est verrouillé en raison de plusieurs tentatives de connexion échouées. Veuillez réessayer plus tard.");
                }

                _logger.LogWarning("Tentative de connexion échouée : mot de passe invalide pour {Email}", request.Email);
                return (false, null, "Email ou mot de passe invalide");
            }

            user.LastLoginAt = DateTime.UtcNow;
            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenRepository.SaveAsync(
                user.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                GetIpAddress(),
                GetDeviceInfo()
            );


            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user, opt =>
            {
                opt.Items["Roles"] = roles.ToList();
            });

            var response = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(15),
                userDto
            );

            _logger.LogInformation("Utilisateur {UserId} connecté avec succès", user.Id);
            return (true, response, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la connexion pour l'email {Email}", request.Email);
            return (false, null, "Une erreur est survenue lors de la connexion. Veuillez réessayer.");
        }
    }


    public async Task<(bool Success, AuthResponse? Response, string? Error)> RegisterAsync(RegisterRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email)) return (false, null, "L'email est requis");
            if (string.IsNullOrWhiteSpace(request.Password)) return (false, null, "Le mot de passe est requis");
            if (request.Password != request.ConfirmPassword) return (false, null, "Les mots de passe ne correspondent pas");

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Tentative d'inscription avec un email existant : {Email}", request.Email);
                return (false, null, "Un compte avec cet email existe déjà");
            }

            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsEmailVerified = false,
                IsPhoneVerified = false,
                ReputationScore = 0,
                ReviewCount = 0
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Échec de l'inscription de l'utilisateur : {Errors}", errors);
                return (false, null, $"Échec de l'inscription : {errors}");
            }

            var roleName = request.RegisterAsDriver ? "Driver" : "Passenger";
            await _userManager.AddToRoleAsync(newUser, roleName);

            var accessToken = _jwtTokenService.GenerateAccessToken(newUser);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenRepository.SaveAsync(
                newUser.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                GetIpAddress(),
                GetDeviceInfo()
            );

            await _userManager.UpdateAsync(newUser);

            try
            {
                await _publishEndpoint.Publish(new UserRegisteredEvent(
                    newUser.Id,
                    newUser.Email!,
                    newUser.FirstName,
                    newUser.LastName,
                    request.RegisterAsDriver,
                    DateTime.UtcNow
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Échec de la publication de l'événement UserRegisteredEvent pour l'utilisateur {UserId}", newUser.Id);
            }

            var roles = await _userManager.GetRolesAsync(newUser);
            var userDto = _mapper.Map<UserDto>(newUser, opt =>
            {
                opt.Items["Roles"] = roles.ToList();
            });

            var response = new AuthResponse(
                accessToken,
                refreshToken,
                DateTime.UtcNow.AddMinutes(15),
                userDto
            );

            _logger.LogInformation("Utilisateur {UserId} inscrit avec succès en tant que {Role}", newUser.Id, roleName);
            return (true, response, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'inscription pour l'email {Email}", request.Email);
            return (false, null, "Une erreur est survenue lors de l'inscription. Veuillez réessayer.");
        }
    }


    public async Task<(bool Success, AuthResponse? Response, string? Error)> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return (false, null, "Le refresh token est requis");

            var storedToken = await _refreshTokenRepository.GetActiveAsync(refreshToken);

            if (storedToken == null)
            {
                _logger.LogWarning("Tentative de refresh avec un token invalide ou expiré");
                return (false, null, "Refresh token invalide");
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null || user.IsDeleted)
            {
                _logger.LogWarning("Utilisateur introuvable pour le refresh token");
                return (false, null, "Refresh token invalide");
            }

            var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenRepository.RevokeAsync(
                refreshToken,
                GetIpAddress(),
                "Token remplacé",
                replacedByToken: newRefreshToken
            );

            await _refreshTokenRepository.SaveAsync(
                user.Id,
                newRefreshToken,
                DateTime.UtcNow.AddDays(7),
                GetIpAddress(),
                GetDeviceInfo()
            );

            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user, opt =>
            {
                opt.Items["Roles"] = roles.ToList();
            });

            var response = new AuthResponse(
                newAccessToken,
                newRefreshToken,
                DateTime.UtcNow.AddMinutes(15),
                userDto
            );

            _logger.LogInformation("Tokens rafraîchis avec succès pour l'utilisateur {UserId}", user.Id);
            return (true, response, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du rafraîchissement des tokens");
            return (false, null, "Une erreur est survenue lors du rafraîchissement des tokens. Veuillez réessayer.");
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null || user.IsDeleted)
            {
                _logger.LogWarning("Utilisateur {UserId} introuvable ou supprimé", id);
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            return _mapper.Map<UserDto>(user, opt =>
            {
                opt.Items["Roles"] = roles.ToList();
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de l'utilisateur {UserId}", id);
            return null;
        }
    }

    public async Task<(bool Success, AuthResponse? Response, string? Error)> LogoutAsync(string refreshToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return (false, null, "Le refresh token est requis");

            await _refreshTokenRepository.RevokeAsync(
                refreshToken,
                GetIpAddress(),
                "Utilisateur déconnecté"
            );

            _logger.LogInformation(
                "Utilisateur déconnecté avec succès pour le token {RefreshToken}",
                refreshToken
            );

            return (true, null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erreur lors de la déconnexion pour le token {RefreshToken}",
                refreshToken
            );

            return (false, null, "Une erreur est survenue lors de la déconnexion. Veuillez réessayer.");
        }
    }


    public async Task<(bool Success, string? Error)> LogoutAllDevicesAsync(Guid userId)
    {
        try
        {
            await _refreshTokenRepository.RevokeAllForUserAsync(userId, "Utilisateur a demandé la déconnexion de tous les appareils");

            _logger.LogInformation("Tous les appareils ont été déconnectés pour l'utilisateur {UserId}", userId);
            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la déconnexion de tous les appareils pour l'utilisateur {UserId}", userId);
            return (false, "Une erreur est survenue lors de la déconnexion de tous les appareils");
        }
    }
}