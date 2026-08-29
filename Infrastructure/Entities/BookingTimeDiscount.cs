namespace Infrastructure.Entities;

public class BookingTimeDiscount
{
    public Guid Id { get; init; }
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
    
    public decimal DiscountPercentage { get; set; }
}