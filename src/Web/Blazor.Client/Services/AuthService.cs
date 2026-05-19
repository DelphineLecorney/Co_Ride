using Blazor.Client.Authentication;
using Blazor.Client.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Contracts.DTOs.Identity;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Blazor.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginRequest form)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", form);

            if (!response.IsSuccessStatusCode)
                return false;

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorageService.SetItemAsync("authToken", result!.AccessToken);

            if (_authenticationStateProvider is CustomAuthStateProvider provider)
                provider.NotifyUserAuthentication();

            return true;
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveItemAsync("authToken");

            if (_authenticationStateProvider is CustomAuthStateProvider provider)
                provider.NotifyUserLogout();

            _httpClient.DefaultRequestHeaders.Authorization = null;

            _navigationManager.NavigateTo("/login");
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is null || !user.Identity.IsAuthenticated)
                return Guid.Empty;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");

            if (userIdClaim is null)
                return Guid.Empty;

            return Guid.TryParse(userIdClaim.Value, out var userId) ? userId : Guid.Empty;
        }
    }
}
