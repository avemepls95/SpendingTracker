using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendingsInDateRange.Contracts;
using SpendingTracker.Common.Primitives;
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
    public Task<GetSpendingsResponseItem[]> Get(
        [FromQuery] Guid userId,
        [FromQuery] int offset,
        [FromQuery] int count,
        CancellationToken cancellationToken)
    {
        var query = new GetSpendingsQuery
        {
            UserId = new UserKey(userId),
            Offset = offset,
            Count = count
        };

        return _mediator.SendQueryAsync<GetSpendingsQuery, GetSpendingsResponseItem[]>(
            query,
            cancellationToken);
    }
}
