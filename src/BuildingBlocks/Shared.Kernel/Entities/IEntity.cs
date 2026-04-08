namespace Shared.Kernel.Entities;

/// <summary>
/// Contrat pour les entités.
/// Dans le contexte de DDD, une entité est un objet qui a 
/// une identité propre et qui peut évoluer au fil du temps.
/// </summary>
public interface IEntity
{
    Guid Id { get; }
}