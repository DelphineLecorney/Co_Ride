namespace Identity.Application.DTOs
{
    /// <summary>
    /// Représente l'objet de transfert de données interne d'un utilisateur, 
    /// contenant des informations essentielles telles que l'identifiant, 
    /// l'adresse e-mail, le nom d'utilisateur, l'état de confirmation de 
    /// l'adresse e-mail, la réputation et l'état de vérification. 
    /// Cet objet DTO est utilisé au sein de l'application pour gérer les opérations
    /// liées aux utilisateurs  et le traitement des données.
    /// </summary>
    public class UserInternalDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public bool EmailConfirmed { get; set; }


        public int Reputation { get; set; }
        public bool IsVerified { get; set; }
    }
}
