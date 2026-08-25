using Domain.Primitives;
using Domain.ValueObjects.Service;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Service : Entity
{
    [JsonConstructor]
    public Service(Guid id) : base(id)
    {
    }

    [JsonProperty] public ServiceName Name { get; private set; }
    [JsonProperty] public decimal Price { get; private set; }

    public static Service Create(ServiceName name, decimal price)
    {
        Guid id = Guid.NewGuid();

        return new Service(id)
        {
            Name = name,
            Price = price
        };
    }
}