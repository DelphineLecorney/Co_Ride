using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.DTOs.Review;

namespace Blazor.Client.Pages.ReviewPages
{
    public partial class UserReviews
    {
        [Inject] ReviewService ReviewService { get; set; } = null!;
        [Parameter] public Guid UserId { get; set; }

        private List<ReviewDto> _reviews = [];
        private double _average = 0;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            _reviews = await ReviewService.GetReviewsByUserAsync(UserId);
            _average = _reviews.Any() ? _reviews.Average(r => r.Rating) : 0;
            _loading = false;
        }
    }
}