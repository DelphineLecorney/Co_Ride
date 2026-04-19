using Shared.Kernel.Domain.Events;

namespace Shared.Kernel.Entities;

public abstract class AggregateRoot : Entity, IAggregateRoot, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _domainEvents.AsReadOnly();
    public void ClearUncommittedEvents() => _domainEvents.Clear();
}

