using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    /// <summary>
    /// Implémentation du repository User
    /// Gère l'accès aux données utilisateur via EF Core
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(IdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted, cancellationToken);

        public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = _userManager.NormalizeEmail(email);
            return await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalized && !u.IsDeleted, cancellationToken);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.LastName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetByRoleAsync(string role, CancellationToken cancellationToken = default)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            return users.Where(u => !u.IsDeleted).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<ApplicationUser>> SearchAsync(string? term, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(term)) return new List<ApplicationUser>();
            term = term.ToUpper();

            return await _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted && (u.NormalizedEmail.Contains(term) || u.LastName.ToUpper().Contains(term)))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            var normalized = _userManager.NormalizeEmail(email);
            return await _context.Users.AnyAsync(u => u.NormalizedEmail == normalized, cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
            => await _context.Users.CountAsync(u => !u.IsDeleted, cancellationToken);


        public async Task CreateAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            CheckIdentityResult(result, "Création");
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            CheckIdentityResult(result, "Mise à jour");
        }


        public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task HardDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                CheckIdentityResult(result, "Suppression définitive");
            }
        }

        private static void CheckIdentityResult(IdentityResult result, string action)
        {
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Échec de la {action} : {errors}");
            }
        }
    }

}
