using System.ComponentModel.DataAnnotations;

namespace WebApi.Booking.Requests;

public class SearchAvailableRoomsRequest
{
    [Required] public required DateOnly Date { get; init; }
    [Required] public required TimeOnly StartTime { get; init; }
    [Required] public required TimeOnly EndTime { get; init; }
    [Required, Range(1, int.MaxValue)] public required int Capacity { get; init; }
}