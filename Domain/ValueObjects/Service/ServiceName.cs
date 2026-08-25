using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects.Room;

namespace Domain.ValueObjects.Service;

public sealed class ServiceName : ValueObject
{
    private const int MaxLength = 30;

    private ServiceName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<ServiceName> Create(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return Result.Failure<ServiceName>(new Error("ServiceName.Empty"));
        }

        if (serviceName.Length > MaxLength)
        {
            return Result.Failure<ServiceName>(new Error("ServiceName.TooLong"));
        }
        
        return new ServiceName(serviceName);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}