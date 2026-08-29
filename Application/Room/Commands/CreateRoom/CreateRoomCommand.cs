using Application.Abstractions;

namespace Application.Room.Commands.CreateRoom;

public record struct CreateRoomCommand(string Name, int Capacity, decimal BaseRentalRate, IReadOnlyList<Guid> Services) : ICommand<Guid>;