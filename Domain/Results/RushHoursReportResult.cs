namespace Domain.Reports;

public class RushHoursReportResult
{
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required IReadOnlyDictionary<TimeOnly, int> OccupiedRooms { get; init; }
}