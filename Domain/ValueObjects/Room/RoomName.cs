using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.Room;

public sealed class RoomName : ValueObject
{
    public const int MaxLength = 30;

    private RoomName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<RoomName> Create(string roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
        {
            return Result.Failure<RoomName>(new Error("RoomName.Empty", "Room name is empty."));
        }

        if (roomName.Length > MaxLength)
        {
            return Result.Failure<RoomName>(new Error("RoomName.TooLong", $"Room name must not exceed {MaxLength} characters."));
        }
        
        return new RoomName(roomName);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    public static implicit operator string(RoomName roomName)
    {
        return roomName.Value;
    }
}