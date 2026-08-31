using Application.Abstractions;
using Domain.Entities;

namespace Application.Room.Queries.GetAllRooms;

public record struct GetAllRoomsQuery : IQuery<List<RoomSnapshot>>;