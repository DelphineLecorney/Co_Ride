namespace Shared.Kernel.Domain.Events
{
    /// <summary>
    /// Cette interface est utilisée pour marquer les entités qui contiennent des événements de domaine.
    /// </summary>
    public interface IHasDomainEvents
    {
        
        // Collection d'événements de domaine associés à l'entité.    
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        // Méthode pour ajouter un événement de domaine à la collection.
        void AddDomainEvent(IDomainEvent domainEvent);

        // Méthode pour supprimer un événement de domaine de la collection.
        void RemoveDomainEvent(IDomainEvent domainEvent);

        // Méthode pour effacer tous les événements de domaine de la collection.
        void ClearDomainEvents();
    }
}