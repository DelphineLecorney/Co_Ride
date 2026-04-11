using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities
{
    /// <summary>
    /// Entité représentant un utilisateur dans le système d'identité.
    /// elle hérite de IdentityUser avec un identifiant de type Guid 
    /// pour une meilleure performance et scalabilité.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
        public string FullName => $"{FirstName} {LastName}";
    }
}
