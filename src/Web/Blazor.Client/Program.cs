using Blazor.Client;
using Blazor.Client.Authentication;
using Blazor.Client.Interfaces;
using Blazor.Client.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();


builder.Services.AddScoped<TripService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ReviewService>();

builder.Services.AddScoped(sp => new HttpClient
{
    // Si je veux en local
    BaseAddress = new Uri("http://localhost:5188/api/")
});

// Si je veux lancer mon docker
//BaseAddress = new Uri("http://localhost:8080")});


await builder.Build().RunAsync();
