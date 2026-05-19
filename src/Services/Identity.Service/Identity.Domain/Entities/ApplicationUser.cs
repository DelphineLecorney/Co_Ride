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
        public UserProfile? Profile { get; private set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";
        public string Initials => $"{(FirstName.Length > 0 ? FirstName[0] : ' ')}{(LastName.Length > 0 ? LastName[0] : ' ')}";

        public bool IsDeleted { get; private set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneVerified { get; set; } = false;

        public decimal ReputationScore { get; private set; } = 0;
        public int ReviewCount { get; private set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void UpdateReputation(decimal score, int reviewCount)
        {
            ReputationScore = score;
            ReviewCount = reviewCount;
        }

        public void RecordLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }

        public UserProfile InitializeProfile()
        {
            Profile = UserProfile.Create(Id, FirstName, LastName, PhoneNumber);
            return Profile;
        }
    }
}
