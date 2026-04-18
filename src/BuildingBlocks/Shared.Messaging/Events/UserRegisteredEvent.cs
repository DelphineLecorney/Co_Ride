namespace Shared.Messaging.Events;

/// <summary>
/// Event publié quand un utilisateur s'inscrit
/// Consommé par: Notification Service
/// </summary>
public record UserRegisteredEvent(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsDriver,
    DateTime RegisteredAt
);