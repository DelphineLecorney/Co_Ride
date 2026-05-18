using Blazor.Client.Interfaces;
using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.Profile;

namespace Blazor.Client.Pages.ProfilePages
{

    public partial class ProfilePage : ComponentBase
    {
        [Inject] private ProfileService ProfileService { get; set; } = default!;
        [Inject] private IAuthService AuthService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private UserProfileDto? _profile;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var userId = await AuthService.GetCurrentUserIdAsync();

                if (userId == Guid.Empty)
                {
                    NavigationManager.NavigateTo("/login");
                    return;
                }

                _profile = await ProfileService.GetProfileByIdAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur de chargement : {ex.Message}");
            }
            finally
            {
                _loading = false;
            }
        }

        private void GoToEdit()
        {
            NavigationManager.NavigateTo("/profile/edit");
        }
    }
}
