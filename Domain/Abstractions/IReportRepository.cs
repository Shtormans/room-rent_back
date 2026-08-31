using Domain.Reports;

namespace Domain.Abstractions;

public interface IReportRepository
{
    public Task<ServicesReportByRoomResult> GetServicesReportByRoom(Guid roomId,
        DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken);

    public Task<RushHoursReportResult> GetRushHoursReport(DateOnly startDate, DateOnly endDate,
        CancellationToken cancellationToken);
}