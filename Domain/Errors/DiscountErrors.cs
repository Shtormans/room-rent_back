using Domain.Shared;

namespace Domain.Errors;

public static class DiscountErrors
{
    public static class Codes
    {
        public const string InvalidStartTime = "Discount.InvalidStartTime";
        public const string InvalidEndTime = "Discount.InvalidEndTime";
        public const string InvalidDiscountPercentage = "Discount.InvalidDiscountPercentage";
        
        public const string NotFound = "Discount.NotFound";
    }

    public static class Helpers
    {
        public static readonly Error InvalidStartTime = new(Codes.InvalidStartTime, "Start time was invalid.");
        public static readonly Error InvalidEndTime = new(Codes.InvalidEndTime, "End time was invalid.");
        public static readonly Error InvalidDiscountPercentage = new(Codes.InvalidDiscountPercentage, "Discount percentage must be greater than 0.");
        
        public static Error NotFound(Guid discountId) => new(Codes.NotFound, $"Discount with id {discountId} was not found.");
    }
}