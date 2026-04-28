using Booking.Domain.Entities;
using Booking.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Persistence
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDbContext _context;

        public BookingRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<BookingEntity?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == Id, cancellationToken);
        }

        public async Task<List<BookingEntity>> GetByPassengerIdAsync(
            Guid passengerId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
                .Where(b => b.PassengerId == passengerId)
                .OrderByDescending(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(BookingEntity booking, CancellationToken cancellationToken = default)
        {
            await _context.Bookings.AddAsync(booking, cancellationToken);
        }

        public async Task<int> CountByPassengerIdAsync(Guid passengerId, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings
            .CountAsync(b => b.PassengerId == passengerId, cancellationToken);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
