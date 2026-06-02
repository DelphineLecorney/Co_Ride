using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.DTOs.Booking;
using Shared.Contracts.DTOs.Trip;

namespace Blazor.Client.Pages.BookingPages
{
    public partial class BookingDetails
    {
        [Inject] BookingService BookingService { get; set; } = null!;
        [Inject] TripService TripService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        [Parameter] public Guid BookinId { get; set; }

        private BookingDto? _booking;
        private TripDto? _trip;
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            _booking = await BookingService.GetBookingByIdAsync(BookinId);

            if (_booking is not null)
                _trip = await TripService.GetTripByIdAsync(_booking.TripId);

            _loading = false;
        }

        private async Task Cancel()
        {
            var success = await BookingService.CancelBookingAsync(BookinId);

            if (success)
                NavigationManager.NavigateTo("/my-bookings");
        }

        private static string GetStatusBadge(string status) => status switch
        {
            "Pending" => "bg-warning text-dark",
            "Confirmed" => "bg-success",
            "Cancelled" => "bg-danger",
            "Completed" => "bg-secondary",
            _ => "bg-light"
        };

    }
}