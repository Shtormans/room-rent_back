using Application.Abstractions;

namespace Application.Room.Commands.UpdateRoomById;

public record struct UpdateRoomByIdCommand(Guid Id, string Name, int Capacity, decimal BaseRentalRate, IReadOnlyList<Guid> Services) : ICommand;