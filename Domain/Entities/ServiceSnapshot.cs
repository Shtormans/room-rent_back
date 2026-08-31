using Domain.Primitives;
using Domain.ValueObjects.Service;

namespace Domain.Entities;

public class ServiceSnapshot(Guid id) : Entity(id)
{
    public required ServiceName Name { get; init; }
    public required ServicePrice Price { get; init; }

    public static ServiceSnapshot Create(ServiceName serviceName, ServicePrice servicePrice)
    {
        Guid id = Guid.NewGuid();

        return new ServiceSnapshot(id)
        {
            Name = serviceName,
            Price = servicePrice
        };
    }
}