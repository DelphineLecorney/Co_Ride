using Booking.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTOs.Trip;
using System.Net.Http.Json;

namespace Booking.Infrastructure.ExternalServices
{


    public class TripServiceClient : ITripServiceClient
    {    
        private readonly HttpClient _httpClient;
        private readonly ILogger<TripServiceClient> _logger;

        public TripServiceClient(HttpClient httpClient, ILogger<TripServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TripDto?> GetTripAsync(Guid tripId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/trips/{tripId}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Échec de la récupération du trajet {TripId}. Statut : {StatusCode}",
                        tripId, response.StatusCode);
                    return null;
                }

                var trip = await response.Content.ReadFromJsonAsync<TripDto>(cancellationToken);
                return trip;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'appel du service Trip pour récupérer le trajet {TripId}", tripId);
                throw;
            }
        }

        public async Task<bool> ReserveSeatsAsync(
            Guid tripId, 
            Guid bookingId, 
            int seatsCount, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new
                {
                    tripId,
                    bookingId,
                    seatsCount
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"api/trips/{tripId}/reserve",
                    request,
                    cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "Echec de la réservation des places. {TripId}, Status: {StatusCode}",
                        tripId, response.StatusCode);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Erreur lors de l'appel du service Trip pour réserver des places. TripId: {TripId}", tripId);
                return false;
            }
        }
    }
}
