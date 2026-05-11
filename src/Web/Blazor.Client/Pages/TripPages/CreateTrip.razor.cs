using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.DTOs.Trip;

namespace Blazor.Client.Pages.TripPages
{
    public partial class CreateTrip
    {
        [Inject] private TripService TripService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private string FromCity { get; set; } = "";
        private string ToCity { get; set; } = "";
        private decimal PricePerSeat { get; set; }
        private DateTime DepartureTime { get; set; } = DateTime.Now.AddDays(1);
        private int AvailableSeats { get; set; }
        private string? Description { get; set; }

        private async Task Create()
        {
            var dto = new CreateTripRequest(
                FromCity,
                ToCity,
                DepartureTime,
                AvailableSeats,
                PricePerSeat,
                Description
            );

            var tripId = await TripService.CreateTripAsync(dto);

            if (tripId != null)
            {
                NavigationManager.NavigateTo("/login");
                return;
            }

            NavigationManager.NavigateTo($"/trip/{tripId}");

        }
    }
}
