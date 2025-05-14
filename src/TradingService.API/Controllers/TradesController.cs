using System.Net.Mime;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TradingService.Application.Dtos;
using TradingService.Application.Features.Trades.Commands.ExecuteTrade;
using TradingService.Application.Features.Trades.Queries.GetPagedTrades;
using TradingService.Application.Features.Trades.Queries.GetTradeById;
using TradingService.Shared.Models;

namespace TradingService.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TradesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TradesController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(typeof(TradeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> Execute([FromBody] ExecuteTradeCommand command)
    {
        var tradeDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTradeById), new { id = tradeDto.Id }, tradeDto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<TradeDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetPagedTrades([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var paginatedResult = await _mediator.Send(new GetPagedTradesQuery(pageNumber, pageSize));
        return Ok(paginatedResult);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TradeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetTradeById(Guid id)
    {
        var tradeDto = await _mediator.Send(new GetTradeByIdQuery(id));
        return Ok(tradeDto);
    }
}
