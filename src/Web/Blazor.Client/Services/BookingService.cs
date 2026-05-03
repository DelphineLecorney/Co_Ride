using Shared.Contracts.DTOs.Booking;
using System.Net.Http.Json;

namespace Blazor.Client.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public BookingService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Guid?> CreateBookingAsync(CreateBookingRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/bookings", request);
                if (response.IsSuccessStatusCode)
                {
                    var bookingId = await response.Content.ReadFromJsonAsync<Guid>();
                    return bookingId;
                }
                else
                {
                    _logger.LogError("Erreur lors de la création de la réservation: {StatusCode}", response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors de la création de la réservation");
                return null;
            }
        }

        public async Task<BookingDto?> GetBookingByIdAsync(Guid bookingId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BookingDto>($"api/bookings/{bookingId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la réservation {BookingId}", bookingId);
                return null;
            }
        }

        public async Task<bool> CancelBookingAsync(Guid bookingId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/bookings/{bookingId}");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError("Erreur lors de l'annulation de la réservation: {StatusCode}", response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception lors de l'annulation de la réservation {BookingId}", bookingId);
                return false;
            }
        }
    }
}