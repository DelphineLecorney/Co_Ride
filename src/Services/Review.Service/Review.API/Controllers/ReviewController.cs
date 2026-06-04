using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Review.Application.Commands.CreateReview;
using Review.Application.Queries.GetReviewsByUser;
using Review.Application.Queries.GetReviewsByTrip;
using Review.Domain.Enums;
using Shared.Contracts.DTOs.Review;
using System.Security.Claims;

namespace Review.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(ISender sender, ILogger<ReviewController> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequest request,
        CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userId, out var reviewerId))
                return Unauthorized();

            var isDriver = User.IsInRole("Driver");

            var command = new CreateReviewCommand(
                request.TripId,
                reviewerId,
                request.RevieweeId,
                isDriver ? ReviewerType.Driver : ReviewerType.Passenger,
                request.Rating,
                request.Comment
            );

            var result = await _sender.Send(command, ct);

            return CreatedAtAction("GetReviewsByUser", new { userId = request.RevieweeId }, result);
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(new
            {
                errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la review");
            return StatusCode(500, new { message = "Une erreur est survenue" });
        }
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetReviewsByUser(
        Guid userId,
        CancellationToken ct)
    {
        var reviews = await _sender.Send(new GetReviewsByUserQuery(userId), ct);

        if (reviews == null)
            return NotFound(new { message = $"Aucune évaluation trouvée pour l'utilisateur {userId}" });

        return Ok(reviews);
    }

    [HttpGet("trip/{tripId:guid}")]
    public async Task<IActionResult> GetReviewsByTrip(
        Guid tripId,
        CancellationToken ct)
    {

        var reviews = await _sender.Send(new GetReviewsByTripQuery(tripId), ct);

        if (reviews == null)
            return NotFound(new { message = $"Aucune évaluation trouvée pour le trajet {tripId}" });

        return Ok(reviews);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyReviews(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var id))
            return Unauthorized();

        var reviews = await _sender.Send(new GetReviewsByUserQuery(id), ct);

        if (reviews == null)
            return NotFound(new { message = "Aucune évaluation trouvée pour votre profil" });

        return Ok(reviews);
    }
}
