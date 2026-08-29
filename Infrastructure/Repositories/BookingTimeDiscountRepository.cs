using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingTimeDiscountRepository(ApplicationDbContext dbContext) : IBookingTimeDiscountRepository
{
    public void Add(BookingTimeDiscountDto discount)
    {
        BookingTimeDiscount entity = ConvertToEntity(discount);
        dbContext.Set<BookingTimeDiscount>().Add(entity);
    }

    public async Task<List<BookingTimeDiscountDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<BookingTimeDiscount>()
            .Select(booking => ConvertToDto(booking))
            .ToListAsync(cancellationToken);
    }

    public async Task<BookingTimeDiscountDto?> GetDiscount(TimeOnly bookTime, CancellationToken cancellationToken)
    {
        BookingTimeDiscount? entity = await dbContext
            .Set<BookingTimeDiscount>()
            .FirstOrDefaultAsync(discount => bookTime.IsBetween(discount.From, discount.To), cancellationToken);

        if (entity == null)
        {
            return null;
        }
        
        return ConvertToDto(entity);
    }
    
    private async Task<BookingTimeDiscount?> GetDatabaseObjectById(Guid id, bool withTracking, CancellationToken cancellationToken)
    {
        IQueryable<BookingTimeDiscount> query = dbContext.Set<BookingTimeDiscount>();
        
        query = withTracking ? query.AsTracking() : query.AsNoTracking();
        return await query.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    
    private static BookingTimeDiscountDto ConvertToDto(BookingTimeDiscount entity)
    {
        return new BookingTimeDiscountDto(entity.Id)
        {
            From = entity.From,
            To = entity.To,
            DiscountPercentage = entity.DiscountPercentage
        };
    }

    private static BookingTimeDiscount ConvertToEntity(BookingTimeDiscountDto dto)
    {
        return new BookingTimeDiscount
        {
            Id = dto.Id,
            From = dto.From,
            To = dto.To,
            DiscountPercentage = dto.DiscountPercentage
        };
    }
}