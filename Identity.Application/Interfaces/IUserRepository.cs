using Identity.Domain.Entities;

namespace Identity.Application.Interfaces
{
    /// <summary>
    /// Cette interface définit les opérations de base pour la gestion 
    /// des utilisateurs dans le système d'identité.
    /// </summary>
    public interface IUserRepository
    {
        // Queries
        Task<ApplicationUser?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<ApplicationUser?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<ApplicationUser?> GetByFullNameAsync(
            string firstName,
            string lastName,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ApplicationUser>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ApplicationUser>> SearchAsync(
            string? searchTerm = null,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<ApplicationUser>> GetByRoleAsync(
            string role,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<bool> FullNameExistsAsync(
            string firstName,
            string lastName,
            CancellationToken cancellationToken = default);


        // Commands
        Task CreateAsync(
            ApplicationUser user,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            ApplicationUser user,
            CancellationToken cancellationToken = default);

        Task SoftDeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}
