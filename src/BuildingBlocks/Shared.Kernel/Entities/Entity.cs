namespace Shared.Kernel.Entities;

/// <summary>
/// Classe de base pour les entités du domaine, 
/// fournissant une implémentation de l'interface IEntity.
/// </summary>
public abstract class Entity : IEntity
{
    public Guid Id { get; protected set; }

    // Constructeur utilisé par les classes dérivées lorsqu'un Id est déjà connu.
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        Id = id;
    }

    // Constructeur requis par EF Core et utilisé pour générer automatiquement un nouvel Id.
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    // Surcharge de Equals pour comparer les entités par leur Id.
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    // Surcharge de GetHashCode pour générer un hash basé sur l'Id.
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // Surcharge de l'opérateur == pour comparer les entités par leur Id.
    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null)
            return right is null;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    // Surcharge de l'opérateur != pour comparer les entités par leur Id.
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}