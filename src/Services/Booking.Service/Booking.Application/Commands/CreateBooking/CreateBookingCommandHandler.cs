using Booking.Application.Interfaces;
using Booking.Domain.Entities;
using Booking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Booking.Application.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, CreateBookingResult>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITripServiceClient _tripServiceClient;
        private readonly ILogger<CreateBookingCommandHandler> _logger;

        public CreateBookingCommandHandler(
            IBookingRepository bookingRepository,
            ITripServiceClient tripServiceClient,
            ILogger<CreateBookingCommandHandler> logger)
        {
            _bookingRepository = bookingRepository;
            _tripServiceClient = tripServiceClient;
            _logger = logger;
        }
        public async Task<CreateBookingResult> Handle(
            CreateBookingCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Récupérer les infos du trip
                var trip = await _tripServiceClient.GetTripAsync(request.TripId, cancellationToken);

                if (trip == null)
                {
                    throw new TripNotFoundException(request.TripId);
                }

                // Vérifier les places disponibles
                if (trip.AvailableSeats < request.SeatsCount)
                {
                    throw new InsufficientSeatsException(request.SeatsCount, trip.AvailableSeats);
                }

                // Calculer le prix
                var totalPrice = trip.PricePerSeat * request.SeatsCount;

                // Créer la réservation
                var bookingId = Guid.NewGuid();
                var booking = BookingEntity.Create(
                    bookingId,
                    request.TripId,
                    request.PassengerId,
                    request.SeatsCount,
                    totalPrice
                    );

                // Sauvegarder la réservation
                await _bookingRepository.AddAsync(booking, cancellationToken);
                await _bookingRepository.SaveChangesAsync(cancellationToken);

                // Réserver les sièges dans le Trip Service
                var reservationSuccess = await _tripServiceClient.ReserveSeatsAsync(
                    request.TripId,
                    bookingId,
                    request.SeatsCount,
                    cancellationToken
                    );

                if (!reservationSuccess)
                {
                    booking.Cancel("Impossible de réserver des sièges.");
                    await _bookingRepository.SaveChangesAsync(cancellationToken);

                    return new CreateBookingResult(
                        bookingId,
                        false,
                        "Impossible de réerver les sièges, la réservation est annulée."
                        );
                }

                // Confirmer la réservation
                booking.COnfirm();
                await _bookingRepository.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "La réservation est bien créée. BookingId: {BookingId}, TripId: {TripId}, Seats: {SeatsCount}",
                    bookingId, request.TripId, request.SeatsCount
                    );

                return new CreateBookingResult(
                    bookingId,
                    true,
                    "Booking created successfully"
                    );
            }
            catch (TripNotFoundException ex)
            {
                _logger.LogWarning(ex, "Trajet pas trouvé : {TripId}", request.TripId);
                return new CreateBookingResult(Guid.Empty, false, ex.Message);
            }
            catch (InsufficientSeatsException ex)
            {
                _logger.LogWarning(ex, "Nombre de places insuffisant pour : {TripId}", request.TripId);
                return new CreateBookingResult(Guid.Empty, false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur de création de la réservation.");
                throw;
            }
        }
    }
}
