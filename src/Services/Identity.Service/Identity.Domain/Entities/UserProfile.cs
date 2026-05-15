namespace Identity.Domain.Entities
{
    // Identity.Domain/Entities/UserProfile.cs
    public class UserProfile
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string DisplayName { get; private set; } = string.Empty;
        public string? Bio { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? PhoneNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // Requis par EF Core
        private UserProfile() { }

        public static UserProfile Create(Guid userId, string displayName, string? phoneNumber = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

            return new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DisplayName = displayName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public void Update(string displayName, string? bio, string? phoneNumber)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

            DisplayName = displayName;
            Bio = bio;
            PhoneNumber = phoneNumber;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvatar(string avatarUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(avatarUrl);

            AvatarUrl = avatarUrl;
            UpdatedAt = DateTime.UtcNow;
        }
    }


}