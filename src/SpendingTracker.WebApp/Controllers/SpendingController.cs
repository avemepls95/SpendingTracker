using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/spending")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class SpendingController : BaseController
{
    private readonly IMediator _mediator;
    
    public SpendingController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("list")]
    public Task<GetSpendingsResponseItem[]> Get(
        [FromQuery] int offset = 0,
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSpendingsQuery
        {
            UserId = GetCurrentUserId(),
            Offset = offset,
            Count = count
        };

        return _mediator.SendQueryAsync<GetSpendingsQuery, GetSpendingsResponseItem[]>(
            query,
            cancellationToken);
    }
}
