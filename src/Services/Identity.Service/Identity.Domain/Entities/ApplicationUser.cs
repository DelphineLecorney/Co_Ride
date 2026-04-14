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

        public string FullName => $"{FirstName} {LastName}";
        public string Initials => $"{(FirstName.Length > 0 ? FirstName[0] : ' ')}{(LastName.Length > 0 ? LastName[0] : ' ')}";

        public bool IsDeleted { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneVerified { get; set; } = false;

        public decimal ReputationScore { get; set; } = 0;
        public int ReviewCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }

}
