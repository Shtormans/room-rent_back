using Application.Abstractions;
using Domain.Reports;

namespace Application.Report.Queries.GetServicesReportByRoom;

public record struct GetServicesReportByRoomQuery(Guid RoomId, DateOnly StartDate, DateOnly EndDate) : IQuery<ServicesReportByRoomResult>;