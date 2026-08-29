namespace WebApi.Discount.Responses;

public class BookingTimeDiscountResponse
{
    public required Guid Id { get; init; }
    public required TimeOnly From { get; set; }
    public required TimeOnly To { get; set; }
    
    public required decimal DiscountPercentage { get; set; }
}