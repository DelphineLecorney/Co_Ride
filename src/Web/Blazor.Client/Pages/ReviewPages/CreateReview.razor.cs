using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Contracts.DTOs.Review;

namespace Blazor.Client.Pages.ReviewPages
{
    public partial class CreateReview
    {
        [Inject] ReviewService ReviewService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
        [Parameter] public Guid TripId { get; set; }
        [Parameter] public Guid RevieweeId { get; set; }

        private int _rating = 0;
        private string? _comment;
        private bool _loading = false;
        private bool _success = false;
        private string? _errorMessage;

        private async Task Submit()
        {
            if (_rating == 0)
            {
                _errorMessage = "Veuillez sélectionner une note.";

                return;
            }

            _loading = true;
            _errorMessage = null;

            var success = await ReviewService.CreateReviewAsync(new CreateReviewRequest(
                TripId,
                RevieweeId,
                _rating,
                _comment
            ));

            _loading = false;

            if (success)
            {
                _success = true;
                await Task.Delay(1500);
                NavigationManager.NavigateTo("/my-bookings");
            }
            else
            {
                _errorMessage = "Impossible d'envoyer l'avis. Réessaie.";
            }
        }

        private string GetRatingLabel() => _rating switch
        {
            1 => "😞 Très mauvais",
            2 => "😐 Mauvais",
            3 => "🙂 Correct",
            4 => "😊 Bien",
            5 => "🤩 Excellent !",
            _ => "Sélectionnez une note"
        };
    }
}
