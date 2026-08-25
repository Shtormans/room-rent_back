using Domain.Primitives;
using Domain.ValueObjects.Room;
using Newtonsoft.Json;

namespace Domain.Entities;

public sealed class Room : Entity
{
    private List<Service> _services;
    
    [JsonConstructor]
    public Room(Guid id) : base(id)
    {
    }
    
    [JsonProperty] public RoomName Name { get; private set; }
    [JsonProperty] public int Capacity { get; private set; }
    [JsonProperty] public decimal BaseRentalRate { get; private set; }
    
    public IReadOnlyList<Service> Services => _services;

    public static Room Create(RoomName name, int capacity, decimal baseRentalRate)
    {
        Guid id = Guid.NewGuid();
        
        return new Room(id)
        {
            Name = name,
            Capacity = capacity,
            BaseRentalRate = baseRentalRate
        };
    }
}