using Domain.Entities;

namespace Domain.Abstractions;

public interface IBookingTimeDiscountRepository
{
    public void Add(BookingTimeDiscountDto discount);
    
    public Task<List<BookingTimeDiscountDto>> GetAll(CancellationToken cancellationToken);
    
    public Task<BookingTimeDiscountDto?> GetDiscount(TimeOnly bookTime, CancellationToken cancellationToken);
}