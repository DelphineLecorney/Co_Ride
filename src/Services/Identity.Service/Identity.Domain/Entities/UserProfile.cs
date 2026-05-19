using Microsoft.EntityFrameworkCore.Metadata;

namespace Identity.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? Bio { get; private set; }
        public string? AvatarUrl { get; private set; }
        public string? PhoneNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private UserProfile() { }

        public static UserProfile Create(Guid userId, string firstName, string lastName, string? phoneNumber = null)
            => new()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

        public void Update(string firstName, string lastName, string? bio, string? phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
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