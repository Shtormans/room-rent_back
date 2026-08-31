using System.ComponentModel.DataAnnotations;

namespace WebApi.Report.Requests;

public class RushHoursReportRequest
{
    [Required] public DateOnly StartDate { get; init; }
    [Required] public DateOnly EndDate { get; init; }
}