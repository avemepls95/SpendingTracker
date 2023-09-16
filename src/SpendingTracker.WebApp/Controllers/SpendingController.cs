using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInRange.Contracts;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/spending")]
public class SpendingController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SpendingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet(Name = "list")]
    public Task<GetSpendingsInRangeResponse> Get(
        [FromQuery] GetSpendingsInRangeQuery inRangeQuery,
        CancellationToken cancellationToken)
    {
        return _mediator.SendQueryAsync<GetSpendingsInRangeQuery, GetSpendingsInRangeResponse>(
            inRangeQuery,
            cancellationToken);
    }
}
