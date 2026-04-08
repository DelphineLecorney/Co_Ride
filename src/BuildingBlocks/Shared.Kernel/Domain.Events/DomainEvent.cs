namespace Shared.Kernel.Domain.Events
{
    /// <summary>
    /// Classe de base pour les événements de domaine, 
    /// fournissant une implémentation de l'interface IDomainEvent.
    /// </summary>
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid EventId { get; private set; }
        public DateTime OccurredOn { get; private set; }
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}