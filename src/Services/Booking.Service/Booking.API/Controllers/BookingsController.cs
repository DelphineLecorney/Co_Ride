using Booking.Application.Commands.CancelBooking;
using Booking.Application.Commands.CreateBooking;
using Booking.Application.Queries.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(IMediator mediator, ILogger<BookingsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateBookingResult), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return CreatedAtAction(
                nameof(GetBookingByIdQuery),
                new { id = result.BookingId },
                result);
        }

        [HttpPost("{id:guid}/cancel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelBooking(
            Guid id,
            [FromBody] CancelBookingCommand command)
        {
            if (id != command.BookingId)
            {
                return BadRequest(new { message = "ID incompatible" });
            }

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(result);
        }
    }
}
