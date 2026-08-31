using Domain.Primitives;

namespace Domain.Entities;

public sealed class BookingTimeDiscountSnapshot(Guid id) : Entity(id)
{
    public required TimeOnly From { get; init; }
    public required TimeOnly To { get; init; }
    public required decimal DiscountPercentage { get; init; }

    public static BookingTimeDiscountSnapshot Create(TimeOnly from, TimeOnly to, decimal discountPercentage)
    {
        Guid id = Guid.NewGuid();

        return new BookingTimeDiscountSnapshot(id)
        {
            From = from,
            To = to,
            DiscountPercentage = discountPercentage
        };
    }
}