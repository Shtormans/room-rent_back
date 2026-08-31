using Application.Report.Queries.GetRushHoursReport;
using Application.Report.Queries.GetServicesReportByRoom;
using Domain.Reports;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Constants;
using WebApi.Report.Requests;

namespace WebApi.Report.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Authorize(Roles = RoleConstants.Admin)]
public class ReportController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    [Route("room-services")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServicesReportByRoom([FromQuery] RoomServicesReportRequest request, CancellationToken cancellationToken)
    {
        GetServicesReportByRoomQuery query = new(request.RoomId, request.StartDate, request.EndDate);
        Result<ServicesReportByRoomResult> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("rush-hours")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRushHoursReport([FromQuery] RushHoursReportRequest request, CancellationToken cancellationToken)
    {
        GetRushHoursReportQuery query = new(request.StartDate, request.EndDate);
        Result<RushHoursReportResult> result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
}