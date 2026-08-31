using System.ComponentModel.DataAnnotations;

namespace WebApi.Report.Requests;

public class RoomServicesReportRequest
{
    [Required] public Guid RoomId { get; init; }
    [Required] public DateOnly StartDate { get; init; }
    [Required] public DateOnly EndDate { get; init; }
}