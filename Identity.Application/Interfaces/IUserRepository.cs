using Identity.Domain.Entities;

namespace Identity.Application.Interfaces
{
    /// <summary>
    /// Cette interface définit les opérations de base pour la gestion 
    /// des utilisateurs dans le système d'identité.
    /// </summary>

    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ApplicationUser>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ApplicationUser>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ApplicationUser>> SearchAsync(string? term, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task CreateAsync(ApplicationUser user, string password);
        Task UpdateAsync(ApplicationUser user);

        Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}