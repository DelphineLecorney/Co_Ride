namespace Trip.Domain.ValueObjects
{
    public class DriverProfile
    {
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string? AvatarUrl { get; private set; }
        public DateTime LastSyncedAt { get; private set; }

        private DriverProfile() { }

        public static DriverProfile CreateFromEvent(
            Guid userId,
            string FirstName,
            string LastName,
            string? avatarUrl)
        => new()
        {
            UserId = userId,
            FirstName = FirstName,
            LastName = LastName,
            AvatarUrl = avatarUrl,
            LastSyncedAt = DateTime.UtcNow
        };

        public void SyncFromEvent(string firstName, string lastName, string? avatarUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            AvatarUrl = avatarUrl;
            LastSyncedAt = DateTime.UtcNow;
        }
    }
}
