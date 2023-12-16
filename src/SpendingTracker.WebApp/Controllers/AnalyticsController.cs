using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Analytics.GetCategoriesAnalyticsByRange.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.WebApp.Contracts.GetCategoriesAnalyticsByRange;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/analytics")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class AnalyticsController : BaseController
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-date-range")]
    public async Task<GetCategoriesAnalyticsByRangeResponse> GetCategoriesAnalyticsByRange(
        [FromQuery] GetCategoriesAnalyticsByRangeRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetCategoriesAnalyticsByRangeQuery
        {
            DateFrom = request.DateFrom,
            DateTo = request.DateTo,
            UserId = GetCurrentUserId(),
            TargetCurrencyId = request.TargetCurrencyId
        };
        
        var result = await _mediator.SendQueryAsync<
            GetCategoriesAnalyticsByRangeQuery,
            GetCategoriesAnalyticsByRangeResponse>(query, cancellationToken);

        return result;
    }
}