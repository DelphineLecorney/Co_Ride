using Shared.Contracts.DTOs.Trip;
using System.Net.Http.Json;

namespace Blazor.Client.Services
{
    public class TripService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public TripService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<TripDto>> SearchTripAsync(
            string? fromCity = null, 
            string? toCity = null, 
            decimal? maxPrice = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(fromCity)) queryParams
                        .Add($"fromCity={Uri.EscapeDataString(fromCity)}");

                if (!string.IsNullOrEmpty(toCity)) queryParams
                        .Add($"toCity={Uri.EscapeDataString(toCity)}");
                if (maxPrice.HasValue) queryParams.Add($"maxPrice={maxPrice}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

                var response = await _httpClient.GetFromJsonAsync<SearchTripsResponse>($"api/trips{query}");

                return response?.Trips ?? new List<TripDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche de voyages");
                return new List<TripDto>();
            }
        }

        public async Task<TripDto?> GetTripByIdAsync(Guid tripId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TripDto>($"api/trips/{tripId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du trajet {TripId}", tripId);
                return null;
            }
        }

        public async Task<Guid?> CreateTripAsync(CreateTripRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/trips", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Guid>();
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du trajet");
                return null;
            }
        }

        public async Task<bool> CancelTripAsync(Guid tripId, Guid driverId, string reason)
        {
            try
            {
                var request = new CancelTripRequest
                {
                    TripId = tripId,
                    DriverId = driverId,
                    Reason = reason
                };

                var response = await _httpClient.PostAsJsonAsync($"api/trips/{tripId}/cancel", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'annulation du trajet");
                return false;
            }
        }
    }
}
