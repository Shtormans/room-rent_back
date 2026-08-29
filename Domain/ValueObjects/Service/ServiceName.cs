using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects.Service;

public sealed class ServiceName : ValueObject
{
    public const int MaxLength = 30;

    private ServiceName(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<ServiceName> Create(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return Result.Failure<ServiceName>(new Error("ServiceName.Empty", "Service name is empty."));
        }

        if (serviceName.Length > MaxLength)
        {
            return Result.Failure<ServiceName>(new Error("ServiceName.TooLong", $"Service name must not exceed {MaxLength} characters."));
        }
        
        return new ServiceName(serviceName);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    public static implicit operator string(ServiceName serviceName)
    {
        return serviceName.Value;
    }
}