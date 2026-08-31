using Domain.Entities;

namespace Domain.Abstractions;

public interface IBookingTimeDiscountRepository
{
    public void Add(BookingTimeDiscountSnapshot discount);
    public Task<List<BookingTimeDiscountSnapshot>> GetAll(CancellationToken cancellationToken);
    
    public Task<bool> TryUpdate(BookingTimeDiscountSnapshot updatediscount, CancellationToken cancellationToken);
    public Task<bool> TryDelete(Guid id, CancellationToken cancellationToken);
    
    public Task<BookingTimeDiscountSnapshot?> GetDiscount(TimeOnly bookTime, CancellationToken cancellationToken);
}