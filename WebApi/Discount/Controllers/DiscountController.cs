using Application.Discount.Commands;
using Application.Discount.Commands.CreateBookingTimeDiscount;
using Application.Discount.Commands.DeleteBookingTimeDiscountById;
using Application.Discount.Commands.UpdateBookingTimeDiscountById;
using Application.Discount.Queries.GetAllBookingTimeDiscounts;
using Domain.Errors;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Constants;
using WebApi.Discount.Requests;
using WebApi.Discount.Responses;

namespace WebApi.Discount.Controllers;

[ApiController]
[Route("api/v1/discounts")]
[Authorize(Roles = RoleConstants.Admin)]
public class DiscountController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBookingTimeDiscount([FromBody] CreateBookingTimeDiscountRequest request,
        CancellationToken cancellationToken)
    {
        CreateBookingTimeDiscountCommand command = new(request.From, request.To, request.DiscountPercentage);
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBookingTimeDiscounts(CancellationToken cancellationToken)
    {
        GetAllBookingTimeDiscountsQuery query = new();
        var result = await Sender.Send(query, cancellationToken);

        List<BookingTimeDiscountResponse> response = result.Value
            .Select(discount => new BookingTimeDiscountResponse
            {
                Id = discount.Id,
                From = discount.From,
                To = discount.To,
                DiscountPercentage = discount.DiscountPercentage,
            }).ToList();

        return Ok(response);
    }
    
    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBookingTimeDiscount([FromRoute] Guid id, [FromBody] UpdateBookingTimeDiscountRequest request, CancellationToken cancellationToken)
    {
        UpdateBookingTimeDiscountByIdCommand command = new(id, request.From, request.To, request.DiscountPercentage);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return result.Error.Code == DiscountErrors.Codes.NotFound 
            ? NotFound(result.Error) 
            : BadRequest(result.Error);
    }
    
    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = RoleConstants.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBookingTimeDiscount(Guid id, CancellationToken cancellationToken)
    {
        DeleteBookingTimeDiscountByIdCommand command = new(id);
        var result = await Sender.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        return NotFound(result.Error);
    }
}