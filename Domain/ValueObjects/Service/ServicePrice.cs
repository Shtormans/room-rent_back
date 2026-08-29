using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.Service;

public class ServicePrice : ValueObject
{
    private ServicePrice(decimal value)
    {
        Value = value;
    }
    
    public decimal Value { get; }

    public static Result<ServicePrice> Create(decimal servicePrice)
    {
        if (servicePrice < 0)
        {
            return Result.Failure<ServicePrice>(new Error("ServicePrice.InvalidRange", "Service price must be greater than or equal to zero."));
        }
        
        return new ServicePrice(servicePrice);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static implicit operator decimal(ServicePrice servicePrice)
    {
        return servicePrice.Value;
    }
}