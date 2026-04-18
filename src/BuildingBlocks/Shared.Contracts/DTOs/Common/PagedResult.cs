namespace Shared.Contracts.DTOs.Common
{
    /// <summary>
    /// Représente un résultat paginé renvoyé par l'API.
    /// </summary>
    /// <typeparam name="T">Type des éléments contenus dans la page.</typeparam>
    /// <param name="Items">Liste des éléments de la page courante.</param>
    /// <param name="TotalCount">Nombre total d'éléments disponibles.</param>
    /// <param name="PageNumber">Numéro de la page actuelle (1-based).</param>
    /// <param name="PageSize">Nombre d'éléments par page.</param>
    public record PagedResult<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize
    )
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
