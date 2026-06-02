using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Shared.Contracts.DTOs.Booking;
using Shared.Contracts.DTOs.Trip;

namespace Blazor.Client.Pages.BookingPages
{
    public partial class MyBookings
    {
        [Inject] BookingService BookingService { get; set; } = null!;
        [Inject] TripService TripService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;

        private List<(BookingDto, TripDto?)> _items = [];
        private bool _loading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadBookings();
            _loading = false;
        }

        private async Task LoadBookings()
        {
            var bookings = await BookingService.GetMyBookingsAsync() ?? [];

            _items = [];
            foreach (var booking in bookings)
            {
                var trip = await TripService.GetTripByIdAsync(booking.TripId);
                _items.Add((booking, trip));
            }
        }

        private async Task Cancel(Guid bookingId)
        {
            await BookingService.CancelBookingAsync(bookingId);
            await LoadBookings();
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