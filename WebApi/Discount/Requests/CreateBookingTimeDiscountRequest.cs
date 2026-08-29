using System.ComponentModel.DataAnnotations;

namespace WebApi.Discount.Requests;

public class CreateBookingTimeDiscountRequest
{
   [Required] public required TimeOnly From { get; set; }
   [Required] public required TimeOnly To { get; set; }
   [Required, Range(0f, 1f)] public required decimal DiscountPercentage { get; set; }
}