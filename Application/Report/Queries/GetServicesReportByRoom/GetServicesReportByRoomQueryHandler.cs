using Application.Abstractions;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;
using Domain.Reports;
using Domain.Shared;

namespace Application.Report.Queries.GetServicesReportByRoom;

public class GetServicesReportByRoomQueryHandler : IQueryHandler<GetServicesReportByRoomQuery, ServicesReportByRoomResult>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IReportRepository _reportRepository;
    
    public GetServicesReportByRoomQueryHandler(IRoomRepository roomRepository, IReportRepository reportRepository)
    {
        _roomRepository = roomRepository;
        _reportRepository = reportRepository;
    }

    public async Task<Result<ServicesReportByRoomResult>> Handle(GetServicesReportByRoomQuery request, CancellationToken cancellationToken)
    {
        if (request.EndDate < request.StartDate)
        {
            return Result.Failure<ServicesReportByRoomResult>(ReportErrors.Helpers.InvalidEndDate);
        }

        RoomSnapshot? snapshot = await _roomRepository.GetById(request.RoomId, cancellationToken);
        if (snapshot is null)
        {
            return Result.Failure<ServicesReportByRoomResult>(RoomErrors.Helpers.NotFound(request.RoomId));
        }

        return await _reportRepository.GetServicesReportByRoom(request.RoomId, request.StartDate, request.EndDate, cancellationToken);
    }
}