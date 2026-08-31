using Application.Abstractions;
using Domain.Entities;

namespace Application.Room.Queries.GetRoomById;

public record struct GetRoomByIdQuery(Guid Id) : IQuery<RoomSnapshot>;