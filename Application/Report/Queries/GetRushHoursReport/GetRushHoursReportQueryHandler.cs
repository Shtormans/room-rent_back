using Application.Abstractions;
using Domain.Abstractions;
using Domain.Errors;
using Domain.Reports;
using Domain.Shared;

namespace Application.Report.Queries.GetRushHoursReport;

public class GetRushHoursReportQueryHandler : IQueryHandler<GetRushHoursReportQuery, RushHoursReportResult>
{
    private readonly IReportRepository _reportRepository;

    public GetRushHoursReportQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<Result<RushHoursReportResult>> Handle(GetRushHoursReportQuery request, CancellationToken cancellationToken)
    {
        if (request.EndDate < request.StartDate)
        {
            return Result.Failure<RushHoursReportResult>(ReportErrors.Helpers.InvalidEndDate);
        }
        
        return await _reportRepository.GetRushHoursReport(request.StartDate, request.EndDate, cancellationToken);
    }
}