
namespace Shared.Messaging.SharedMessagingDomainEvents;

/// <summary>
/// Cette classe définit les événements liés aux trajets (trips) dans le système de covoiturage.
/// Elle contient des records pour les événements de création, de publication et d'annulation de trajet,
/// permettant ainsi de structurer et de centraliser la gestion des événements liés aux trajets.
/// </summary>
public class TripEvents
{
    public record TripCreatedEvent(
        Guid TripId,
        Guid DriverId,
        string FromCity,
        string ToCity,
        DateTime DepartureTime,
        int AvailableSeats,
        decimal PricePerSeat
    );

    public record TripPublishedEvent(
        Guid TripId,
        DateTime PublishedAt
    );

    public record TripCancelledEvent(
        Guid TripId,
        string Reason,
        DateTime CancelledAt
    );
}

