using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.Profile;
using System.ComponentModel.DataAnnotations;

namespace Blazor.Client.Pages.ProfilePages
{
    public partial class EditProfilePage : ComponentBase
    {
        [Inject] private ProfileService ProfileService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;


        private EditProfileModel _model = new();
        private bool _loading = true;
        private bool _saving = false;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            var profile = await ProfileService.GetMyProfileAsync();

            if (profile is not null)
            {
                _model.Bio = profile.Bio;
                _model.PhoneNumber = profile.PhoneNumber;
            }

            _loading = false;
        }

        private async Task HandleSubmit()
        {
            _saving = true;
            _errorMessage = null;

            var success = await ProfileService.UpdateProfileAsync(new UpdateUserProfileRequest(
                _model.FirstName,
                _model.LastName,
                _model.Bio,
                _model.PhoneNumber
            ));

            _saving = false;

            if (success)
                NavigationManager.NavigateTo("/profile");
            else
                _errorMessage = "Une erreur est survenue. Réessaie.";
        }

        private void Cancel() => NavigationManager.NavigateTo("/profile");


        private class EditProfileModel
        {
            [Required(ErrorMessage = "Le prénom est obligatoire.")]
            [MaxLength(50)]
            public string FirstName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le nom est obligatoire.")]
            [MaxLength(50)]
            public string LastName { get; set; } = string.Empty;

            [MaxLength(500)]
            public string? Bio { get; set; }

            [MaxLength(20)]
            public string? PhoneNumber { get; set; }
        }
    }
}