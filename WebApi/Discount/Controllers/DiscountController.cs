using Application.Discount.Commands;
using Application.Discount.Queries.GetAllBookingTimeDiscounts;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Abstractions;
using WebApi.Discount.Requests;
using WebApi.Discount.Responses;

namespace WebApi.Discount.Controllers;

[ApiController]
[Route("api/v1/discounts")]
public class DiscountController(ISender sender) : ApiController(sender)
{
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
}