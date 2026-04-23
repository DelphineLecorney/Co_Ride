using MediatR;

namespace Trip.Application.Commands.CancelTrip;

/// <summary>
/// Commande pour annuler un trip existant.
/// </summary>
public record CancelTripCommand(
    Guid TripId,
    Guid DriverId,
    string Reason
) : IRequest<CancelTripResponse>;
