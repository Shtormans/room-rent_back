using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BookingRepository(ApplicationDbContext dbContext) : IBookingRepository
{
    public void Add(BookingDto booking)
    {
        Booking entity = ConvertToEntity(booking);
        dbContext.Set<Booking>().Add(entity);
    }

    public async Task<List<BookingDto>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<Booking>()
            .Include(booking => booking.BookingServices)
            .Select(booking => ConvertToDto(booking))
            .ToListAsync(cancellationToken);
    }

    public async Task<BookingDto?> GetById(Guid id, CancellationToken cancellationToken)
    {
        Booking? entity = await GetDatabaseObjectById(id, false, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        return ConvertToDto(entity);
    }

    public async Task<bool> TryDelete(Guid id, CancellationToken cancellationToken)
    {
        Booking? entity = await GetDatabaseObjectById(id, false, cancellationToken);
        
        if (entity is null)
        {
            return false;
        }

        dbContext.Set<Booking>().Remove(entity);
        return true;
    }

    public async Task<List<Guid>> GetAvailableRooms(DateTime start, DateTime end, int capacity,
        CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<Room>()
            .AsNoTracking()
            .Where(room => room.Capacity >= capacity)
            .Where(room => !room.Bookings.Any(booking => 
                start < booking.End && end > booking.Start))
            .Select(room => room.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> CalculateBaseBookingPrice(Guid roomId, int durationHours, IReadOnlyList<Guid> selectedServices, CancellationToken cancellationToken)
    {
        bool hasServices = selectedServices is { Count: > 0 };

        var result = await dbContext
            .Set<Room>()
            .AsNoTracking()
            .Where(room => room.Id == roomId)
            .Select(room => new
            {
                BaseRate = room.BaseRentalRate,
                ServicesPriceSum = hasServices 
                    ? room.RoomServices
                        .Where(rs => selectedServices.Contains(rs.ServiceId))
                        .Sum(rs => (decimal?)rs.Service.Price) ?? 0m
                    : 0m
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            return 0;
        }

        return (result.BaseRate * durationHours) + result.ServicesPriceSum;
    }

    private async Task<Booking?> GetDatabaseObjectById(Guid id, bool withTracking, CancellationToken cancellationToken)
    {
        IQueryable<Booking> query = dbContext
            .Set<Booking>()
            .Include(booking => booking.BookingServices);
        
        query = withTracking ? query.AsTracking() : query.AsNoTracking();
        return await query.FirstOrDefaultAsync(booking => booking.Id == id, cancellationToken);
    }
    
    private static BookingDto ConvertToDto(Booking entity)
    {
        return new BookingDto(entity.Id)
        {
            RoomId = entity.RoomId,
            Start = entity.Start,
            End = entity.End,
            Price = entity.Price,
            Services = entity.BookingServices.Select(bs => bs.ServiceId).ToList()
        };
    }

    private static Booking ConvertToEntity(BookingDto dto)
    {
        List<BookingService> bookingServices = dto
            .Services
            .Select(serviceId => new BookingService
            {
                BookingId = dto.Id,
                ServiceId = serviceId
            }).ToList();

        return new Booking
        {
            Id = dto.Id,
            RoomId = dto.RoomId,
            Start = dto.Start,
            End = dto.End,
            Price = dto.Price,
            BookingServices = bookingServices
        };
    }
}