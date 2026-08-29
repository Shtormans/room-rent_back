using Application.Abstractions;

namespace Application.Room.Commands.DeleteRoomById;

public record struct DeleteRoomByIdCommand(Guid Id) : ICommand;