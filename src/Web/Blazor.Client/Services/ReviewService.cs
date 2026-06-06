using Shared.Contracts.DTOs.Review;
using System.Net.Http.Json;

namespace Blazor.Client.Services
{
    public class ReviewService(HttpClient httpClient, ILogger<ReviewService> logger)
    {
        public async Task<bool> CreateReviewAsync(CreateReviewRequest request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/review", request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la création de l'avis");
                return false;
            }
        }

        public async Task<List<ReviewDto>> GetReviewsByUserAsync(Guid userId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<ReviewDto>>($"api/review/user/{userId}")
                    ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération des avis");
                return [];
            }
        }

        public async Task<List<ReviewDto>> GetReviewsByTripAsync(Guid tripId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<ReviewDto>>($"api/review/trip/{tripId}")
                       ?? [];
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erreur lors de la récupération des avis du trajet");
                return [];
            }
        }
    }
}
