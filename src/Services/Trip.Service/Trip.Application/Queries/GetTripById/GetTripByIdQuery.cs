using MediatR;
using Trip.Application.Projections.ReadModels;

namespace Trip.Application.Queries.GetTripById;

/// <summary>
/// Query pour récupérer un trip par son ID.
/// Retourne le Read Model optimisé pour l'affichage.
/// </summary>
public record GetTripByIdQuery(Guid TripId) : IRequest<TripReadModel?>;