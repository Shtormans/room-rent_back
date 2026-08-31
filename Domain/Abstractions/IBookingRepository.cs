using Domain.Entities;
using Domain.Shared;

namespace Domain.Abstractions;

public interface IBookingRepository
{
    public void Add(BookingSnapshot booking);
    
    public Task<List<BookingSnapshot>> GetAll(CancellationToken cancellationToken);
    public Task<BookingSnapshot?> GetById(Guid id, CancellationToken cancellationToken);
    
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
    
    public Task<List<Guid>> GetAvailableRooms(DateTime start, DateTime end, int capacity, CancellationToken cancellationToken);
    public Task<decimal> CalculateBaseBookingPrice(Guid roomId, int durationHours, IReadOnlyList<Guid> selectedServices, CancellationToken cancellationToken);
}