
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Cette classe implémente le service de génération de tokens JWT pour l'application Identity.
/// elle utilise les paramètres de configuration pour créer des tokens d'accès et de rafraîchissement,
/// et fournit une méthode pour valider les tokens expirés et extraire les informations de l'utilisateur.
/// </summary>
namespace Identity.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Génère un token d'accès JWT pour un utilisateur donné, en incluant les revendications nécessaires.
        /// </summary>
        /// <param name="user">L'utilisateur pour lequel générer le token.</param>
        /// <returns>Le token d'accès JWT.</returns>
        public string GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Génère un token de rafraîchissement sécurisé en utilisant un générateur 
        /// de nombres aléatoires cryptographiquement sécurisé.
        /// </summary>
        /// <returns>Le token de rafraîchissement sécurisé.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Valide un JWT expiré et renvoie le principal associé aux revendications si 
        /// le jeton est structurellement valide et signé avec l'algorithme attendu.
        /// </summary>
        /// <remarks>Cette méthode ne valide pas la durée de vie du jeton, ce qui permet d'extraire 
        /// des revendications à partir de jetons expirés. Elle est généralement utilisée pour récupérer 
        /// des revendications dans le cadre de scénarios de renouvellement ou d'actualisation de jetons. 
        /// Le jeton doit être signé avec la clé secrète configurée et utiliser l'algorithme HMAC SHA-256 ; 
        /// sinon, la méthode renvoiera null.</remarks>
        /// <param name="token">Le JWT expiré sous forme de chaîne. Doit être une valeur non nulle et non 
        /// vide représentant un jeton émis précédemment. </param>
        /// <returns>Un <see cref="ClaimsPrincipal"/> extrait du jeton si la validation réussit et si le 
        /// jeton est signé avec HMAC SHA-256 ; sinon, <see langword="null"/>.</returns>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}

    
