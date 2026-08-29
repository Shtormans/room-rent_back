using Domain.Primitives;
using Domain.ValueObjects.Service;

namespace Domain.Entities;

public class ServiceDto(Guid id) : Entity(id)
{
    public required ServiceName Name { get; init; }
    public required ServicePrice Price { get; init; }

    public static ServiceDto Create(ServiceName serviceName, ServicePrice servicePrice)
    {
        Guid id = Guid.NewGuid();

        return new ServiceDto(id)
        {
            Name = serviceName,
            Price = servicePrice
        };
    }
}