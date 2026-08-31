using Domain.Abstractions;
using Domain.Entities;
using Domain.Reports;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Infrastructure.Repositories;

public class ReportRepository(ApplicationDbContext dbContext) : IReportRepository
{
    public async Task<ServicesReportByRoomResult> GetServicesReportByRoom(Guid roomId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        DateTime start = startDate.ToDateTime(TimeOnly.MinValue);
        DateTime end = endDate.ToDateTime(TimeOnly.MaxValue);

        int totalRoomBookings = await dbContext.Set<Booking>()
            .AsNoTracking()
            .Where(b => b.RoomId == roomId && b.Start >= start && b.End <= end)
            .CountAsync(cancellationToken);

        Dictionary<Guid, int> serviceCounts = await dbContext.Set<RoomService>()
            .AsNoTracking()
            .Where(rs => rs.RoomId == roomId)
            .Select(rs => new
            {
                rs.ServiceId,
                Count = dbContext.Set<BookingService>().Count(bs => bs.ServiceId == rs.ServiceId 
                                                                    && bs.Booking.RoomId == roomId 
                                                                    && bs.Booking.Start >= start 
                                                                    && bs.Booking.End <= end)
            })
            .ToDictionaryAsync(
                x => x.ServiceId, 
                x => x.Count, 
                cancellationToken);

        return new ServicesReportByRoomResult
        {
            RoomId = roomId,
            StartDate = startDate,
            EndDate = endDate,
            BookingsAmount = totalRoomBookings,
            ServiceBookings = serviceCounts
        };
    }

    public async Task<RushHoursReportResult> GetRushHoursReport(DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken)
    {
        DateTime rangeStart = startDate.ToDateTime(TimeOnly.MinValue);
        DateTime rangeEnd = endDate.ToDateTime(TimeOnly.MaxValue);

        var bookings = await dbContext.Set<Booking>()
            .AsNoTracking()
            .Where(b => b.Start < rangeEnd && b.End > rangeStart)
            .Select(b => new { b.Start, b.End })
            .ToListAsync(cancellationToken);

        Dictionary<TimeOnly, int> hourlyOccupancy = Enumerable.Range(0, 24)
            .Select(h => new TimeOnly(h, 0))
            .ToDictionary(
                hour => hour,
                hour => bookings.Count(b => 
                    b.Start.TimeOfDay <= hour.ToTimeSpan() && 
                    b.End.TimeOfDay > hour.ToTimeSpan()
                )
            );

        return new RushHoursReportResult
        {
            StartDate = startDate,
            EndDate = endDate,
            OccupiedRooms = hourlyOccupancy
        };
    }
}