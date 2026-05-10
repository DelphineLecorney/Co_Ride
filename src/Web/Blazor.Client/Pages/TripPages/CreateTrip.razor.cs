using Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Client.Pages.TripPages
{
    public partial class CreateTrip
    {
        [Inject] private TripService TripService { get; set; } = null!;
        [Inject] private AuthService AuthService { get; set; } = null!;
    }
}
