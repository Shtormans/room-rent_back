namespace Domain.Reports;

public class ServicesReportByRoomResult
{
    public required Guid RoomId { get; init; }
    public required DateOnly StartDate { get; init; } 
    public required DateOnly EndDate { get; init; }
    public required int BookingsAmount { get; init; }
    public required IReadOnlyDictionary<Guid, int> ServiceBookings { get; init; }
}