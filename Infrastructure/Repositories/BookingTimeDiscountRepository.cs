using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingTimeDiscountRepository(ApplicationDbContext dbContext) : IBookingTimeDiscountRepository
{
    public void Add(BookingTimeDiscountSnapshot discount)
    {
        BookingTimeDiscount entity = ConvertToEntity(discount);
        dbContext.Set<BookingTimeDiscount>().Add(entity);
    }

    public async Task<List<BookingTimeDiscountSnapshot>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<BookingTimeDiscount>()
            .AsNoTracking()
            .Select(booking => ConvertToSnapshot(booking))
            .ToListAsync(cancellationToken);
    }
    
    public async Task<bool> TryUpdate(BookingTimeDiscountSnapshot updatedService, CancellationToken cancellationToken)
    {
        BookingTimeDiscount? entity = await GetDatabaseObjectById(updatedService.Id, true, cancellationToken);
        
        if (entity is null)
        {
            return false;
        }
        
        entity.From = updatedService.From;
        entity.To = updatedService.To;
        entity.DiscountPercentage = updatedService.DiscountPercentage;

        return true;
    }

    public async Task<bool> TryDelete(Guid id, CancellationToken cancellationToken)
    {
        BookingTimeDiscount? snapshot = await GetDatabaseObjectById(id, false, cancellationToken);
        
        if (snapshot is null)
        {
            return false;
        }

        dbContext.Set<BookingTimeDiscount>().Remove(snapshot);
        return true;
    }

    public async Task<BookingTimeDiscountSnapshot?> GetDiscount(TimeOnly bookTime, CancellationToken cancellationToken)
    {
        BookingTimeDiscount? entity = await dbContext
            .Set<BookingTimeDiscount>()
            .AsNoTracking()
            .FirstOrDefaultAsync(discount => bookTime.IsBetween(discount.From, discount.To), cancellationToken);

        if (entity == null)
        {
            return null;
        }
        
        return ConvertToSnapshot(entity);
    }
    
    private async Task<BookingTimeDiscount?> GetDatabaseObjectById(Guid id, bool withTracking, CancellationToken cancellationToken)
    {
        IQueryable<BookingTimeDiscount> query = dbContext.Set<BookingTimeDiscount>();
        
        query = withTracking ? query.AsTracking() : query.AsNoTracking();
        return await query.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }
    
    private static BookingTimeDiscountSnapshot ConvertToSnapshot(BookingTimeDiscount entity)
    {
        return new BookingTimeDiscountSnapshot(entity.Id)
        {
            From = entity.From,
            To = entity.To,
            DiscountPercentage = entity.DiscountPercentage
        };
    }

    private static BookingTimeDiscount ConvertToEntity(BookingTimeDiscountSnapshot snapshot)
    {
        return new BookingTimeDiscount
        {
            Id = snapshot.Id,
            From = snapshot.From,
            To = snapshot.To,
            DiscountPercentage = snapshot.DiscountPercentage
        };
    }
}