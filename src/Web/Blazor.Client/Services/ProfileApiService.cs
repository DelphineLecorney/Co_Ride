using Shared.Contracts.Profile;
using System.Net.Http.Json;

namespace Blazor.Client.Services
{
    public class ProfileApiService(HttpClient http)
    {
        public Task<UserProfileDto?> GetProfileAsync(Guid userId)
            => http.GetFromJsonAsync<UserProfileDto>($"api/profile/{userId}");

        public Task UpdateProfileAsync(UpdateUserProfileRequest request)
            => http.PutAsJsonAsync("api/profile", request);
    }
}
