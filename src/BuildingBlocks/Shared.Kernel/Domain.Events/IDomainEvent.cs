using MediatR;

namespace Shared.Kernel.Domain.Events
{
    /// <summary>
    /// Interface pour les événements de domaine, qui sont des notifications 
    /// indiquant qu'un événement significatif s'est produit dans le domaine métier.
    /// </summary>
    public interface IDomainEvent : INotification
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }

}
