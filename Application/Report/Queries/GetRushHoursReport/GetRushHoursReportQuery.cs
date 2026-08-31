using Application.Abstractions;
using Domain.Reports;

namespace Application.Report.Queries.GetRushHoursReport;

public record struct GetRushHoursReportQuery(DateOnly StartDate, DateOnly EndDate) : IQuery<RushHoursReportResult>;