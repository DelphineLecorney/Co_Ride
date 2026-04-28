using Booking.Application.Interfaces;
using Booking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Booking.Application.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, CancelBookingResult>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<CancelBookingCommandHandler> _logger;

        public CancelBookingCommandHandler(
            IBookingRepository bookingRepository,
            ILogger<CancelBookingCommandHandler> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task<CancelBookingResult> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Récupérer la réservation
                var booking = await _bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

                if(booking == null)
                {
                    throw new BookingNotFoundException(request.BookingId);
                }

                // Vérifier que c'est bien le passager
                if (booking.PassengerId != request.PassengerId)
                {
                    return new CancelBookingResult(
                        false,
                        "Vous pouvez seulement annuler vos propres réservations."
                        );
                }

                // Annuler la réservation
                booking.Cancel(request.Reason);
                await _bookingRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Réservation annulée. BookingId: {BookingId}, Raison: {Reason}",
                    request.BookingId, request.Reason
                    );
                return new CancelBookingResult(true, "Réservation annulée avec succès.");
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogWarning(ex, "Réservation non trouvée: {BookingId}", request.BookingId);
                return new CancelBookingResult(false, ex.Message);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Impossible d'annuler la réservation: {BookinId}", request.BookingId);
                return new CancelBookingResult(false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erreur d'annulation de la réservation.");
                throw;
            }
        }
    }
}
