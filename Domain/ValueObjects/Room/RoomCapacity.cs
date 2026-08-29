using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects.Service;

namespace Domain.ValueObjects.Room;

public class RoomCapacity : ValueObject
{
    private RoomCapacity(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public static Result<RoomCapacity> Create(int roomCapacity)
    {
        if (roomCapacity < 0)
        {
            return Result.Failure<RoomCapacity>(new Error("RoomCapacity.InvalidRange", "Room capacity must be greater than zero."));
        }
        
        return new RoomCapacity(roomCapacity);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator int(RoomCapacity capacity)
    {
        return capacity.Value;
    }
}