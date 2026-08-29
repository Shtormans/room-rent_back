using Domain.Shared;

namespace Domain.Errors;

public static class ServiceErrors
{
    public static class Codes
    {
        public const string NotFound = "Service.NotFound";
    }

    public static class Helpers
    {
        public static Error NotFound(Guid serviceId) => new(Codes.NotFound, $"Service with id {serviceId} was not found.");
    }
}