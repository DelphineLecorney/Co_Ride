using Shared.Contracts.Profile;
using System.Net.Http.Json;

namespace Blazor.Client.Services
{
    public class ProfileService(HttpClient http)
    {
        public async Task<UserProfileDto?> GetMyProfileAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await http.GetFromJsonAsync<UserProfileDto>("api/profile", cancellationToken);
            }
            catch { return null; }
        }

        public async Task<UserProfileDto?> GetProfileByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await http.GetFromJsonAsync<UserProfileDto>($"api/profile/{userId}", cancellationToken);
            }
            catch { return null; }
        }

        public async Task<bool> UpdateProfileAsync(UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
        {
            var response = await http.PutAsJsonAsync("api/profile", request, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SetAvatarAsync(string avatarUrl, CancellationToken cancellationToken= default)
        {
            var response = await http.PutAsJsonAsync("api/profile/avatar", new SetAvatarRequest(avatarUrl), cancellationToken);
            return response.IsSuccessStatusCode;
        }
    }
}
