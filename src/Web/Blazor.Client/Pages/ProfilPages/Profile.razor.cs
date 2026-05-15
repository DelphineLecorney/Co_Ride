using Microsoft.AspNetCore.Components;

namespace Blazor.Client.Pages.UserPages
{
    public partial class Profile : ComponentBase
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        protected async Task Create()
        {
            Console.WriteLine($"Création du profil : {FirstName} {LastName}");

        }
    }
}
