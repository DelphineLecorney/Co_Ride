using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Contracts.DTOs.Trip;
using System.Security.Claims;

namespace Blazor.Client.Pages.TripPages
{
    public partial class SearchTrips
    {
        [Inject] private TripService TripService { get; set; } = null!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        protected string Name { get; set; } = "Utilisateur";
        protected string FromCity { get; set; } = "";
        protected string ToCity { get; set; } = "";
        protected decimal? MaxPrice { get; set; }

        protected List<TripDto> Trips { get; set; } = new();
        protected bool IsLoading { get; set; }
        protected bool HasSearched { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = auth.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                Name = user.FindFirst(ClaimTypes.Name)?.Value ?? "Utilisateur";
            }
        }

        protected async Task Search()
        {
            IsLoading = true;
            HasSearched = true;

            Trips = await TripService.SearchTripAsync(
                FromCity,
                ToCity,
                MaxPrice
            );

            IsLoading = false;
        }
    }
}
