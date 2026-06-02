using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Contracts.DTOs.Booking;
using Shared.Contracts.DTOs.Trip;
using System.Security.Claims;

namespace Blazor.Client.Pages.BookingPages
{
    public partial class BookingConfirmation
    {
        [Inject] BookingService BookingService { get; set; } = null!;
        [Inject] TripService TripService { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Parameter] public Guid TripId { get; set; }

        private TripDto? _trip;
        private int _seats = 1;
        private bool _loading = true;
        private bool _success = false;
        private string? _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            _trip = await TripService.GetTripByIdAsync(TripId);
        }

        private async Task Confirm()
        {
            _loading = true;
            _errorMessage = null;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var userIdClaim = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var passengerId))
            {
                _errorMessage = "Utilisateur non authentifié.";
                _loading = false;
                return;
            }

            var bookingId = await BookingService.CreateBookingAsync(
                new CreateBookingRequest(
                    TripId,
                    passengerId,
                    _seats
                ));

            _loading = false;

            if (bookingId is not null)
            {
                _success = true;
                await Task.Delay(1500);
                NavigationManager.NavigateTo("/my-bookings");
            }
            else
            {
                _errorMessage = "Impossible de réserver. Réessaie.";
            }
        }
    }
}