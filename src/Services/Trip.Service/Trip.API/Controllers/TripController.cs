using MediatR;
using Microsoft.AspNetCore.Mvc;
using Trip.Application.Commands.CreateTrip;
using Trip.Application.Queries.GetTripById;
using Trip.Application.Queries.SearchTrips;
using Trip.Application.Projections.ReadModels;
using Trip.Application.Commands.CancelTrip;

namespace Trip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TripController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TripController> _logger;

    public TripController(IMediator mediator, ILogger<TripController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Créer un nouveau trip
    /// </summary>
    /// <param name="command">Détails du trip à créer</param>
    /// <returns>ID du trip créé</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripCommand command)
    {
        try
        {
            var tripId = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetTripById),
                new { id = tripId },
                tripId);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new
            {
                errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du trip");
            return StatusCode(500, new { message = "Une erreur est survenue" });
        }
    }

    /// <summary>
    /// Récupérer un trip par son ID
    /// </summary>
    /// <param name="id">ID du trip</param>
    /// <returns>Détails du trip</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TripReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById(Guid id)
    {
        var query = new GetTripByIdQuery(id);
        var trip = await _mediator.Send(query);

        if (trip == null)
        {
            return NotFound(new { message = $"Trip {id} non trouvé" });
        }

        return Ok(trip);
    }

    /// <summary>
    /// Rechercher des trips avec des filtres
    /// </summary>
    /// <param name="fromCity">Ville de départ</param>
    /// <param name="toCity">Ville d'arrivée</param>
    /// <param name="departureDate">Date de départ</param>
    /// <param name="minSeats">Nombre minimum de places</param>
    /// <param name="maxPrice">Prix maximum par place</param>
    /// <param name="page">Numéro de page (défaut: 1)</param>
    /// <param name="pageSize">Taille de page (défaut: 20)</param>
    /// <param name="sortBy">Champ de tri (DepartureTime, Price)</param>
    /// <param name="ascending">Ordre croissant (défaut: true)</param>
    /// <returns>Liste paginée de trips</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SearchTripsResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchTrips(
        [FromQuery] string? fromCity,
        [FromQuery] string? toCity,
        [FromQuery] DateTime? departureDate,
        [FromQuery] int? minSeats,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sortBy = "DepartureTime",
        [FromQuery] bool ascending = true)
    {
        var query = new SearchTripsQuery
        {
            FromCity = fromCity,
            ToCity = toCity,
            DepartureDate = departureDate,
            MinSeats = minSeats,
            MaxPrice = maxPrice,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy,
            Ascending = ascending
        };

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Annuler un trip
    /// </summary>
    /// <param name="id">ID du trip à annuler</param>
    /// <param name="command">Détails de l'annulation</param>
    /// <returns>Résultat de l'annulation</returns>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(CancelTripResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTrip(
        Guid id,
        [FromBody] CancelTripCommand command)
    {
        if (id != command.TripId)
        {
            return BadRequest(new { message = "L'ID dans l'URL ne correspond pas à l'ID dans la commande" });
        }

        try
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new
            {
                errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'annulation du trip {TripId}", id);
            return StatusCode(500, new { message = "Une erreur est survenue" });
        }
    }

    /// <summary>
    /// Réserver des sièges dans un trip (généralement appelé par le Booking Service)
    /// </summary>
    /// <param name="id">ID du trip</param>
    /// <param name="command">Détails de la réservation</param>
    /// <returns>Résultat de la réservation</returns>
    [HttpPost("{id:guid}/reserve")]
    [ProducesResponseType(typeof(Application.Commands.ReserveSeats.ReserveSeatsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReserveSeats(
        Guid id,
        [FromBody] Application.Commands.ReserveSeats.ReserveSeatsCommand command)
    {
        if (id != command.TripId)
        {
            return BadRequest(new { message = "L'ID dans l'URL ne correspond pas à l'ID dans la commande" });
        }

        try
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new
            {
                errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la réservation de sièges pour le trip {TripId}", id);
            return StatusCode(500, new { message = "Une erreur est survenue" });
        }
    }
}