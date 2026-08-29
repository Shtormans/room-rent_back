using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects.Service;

namespace Domain.ValueObjects.Room;

public class RoomRentalRate : ValueObject
{
    private RoomRentalRate(decimal value)
    {
        Value = value;
    }
    
    public decimal Value { get; }

    public static Result<RoomRentalRate> Create(decimal roomRentalRate)
    {
        if (roomRentalRate < 0)
        {
            return Result.Failure<RoomRentalRate>(new Error("RoomRentalRate.InvalidRange", "Room rental rate must be greater than or equal to zero."));
        }
        
        return new RoomRentalRate(roomRentalRate);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator decimal(RoomRentalRate rentalRate)
    {
        return rentalRate.Value;
    }
}