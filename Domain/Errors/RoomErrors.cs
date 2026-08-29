using Domain.Shared;

namespace Domain.Errors;

public static class RoomErrors
{
    public static class Codes
    {
        public const string NotFound = "Room.NotFound";
    }

    public static class Helpers
    {
        public static Error NotFound(Guid roomId) => new(Codes.NotFound, $"Room with id {roomId} was not found.");
    }
}