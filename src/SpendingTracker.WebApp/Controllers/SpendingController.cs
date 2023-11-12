using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Spending.AddSpendingToExistCategory.Contracts;
using SpendingTracker.Application.Handlers.Spending.AddSpendingToNewCategory.Contracts;
using SpendingTracker.Application.Handlers.Spending.DeleteSpending.Contracts;
using SpendingTracker.Application.Handlers.Spending.GetSpendings.Contracts;
using SpendingTracker.Application.Handlers.Spending.RemoveSpendingFromCategory.Contracts;
using SpendingTracker.Application.Handlers.Spending.UpdateSpending.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.WebApp.Contracts.AddSpendingToExistCategory;
using SpendingTracker.WebApp.Contracts.AddSpendingToNewCategory;
using SpendingTracker.WebApp.Contracts.DeleteSpending;
using SpendingTracker.WebApp.Contracts.RemoveFromCategory;
using SpendingTracker.WebApp.Contracts.UpdateSpending;

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
        [FromQuery] Guid? currencyId = null,
        [FromQuery] int offset = 0,
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSpendingsQuery
        {
            UserId = GetCurrentUserId(),
            TargetCurrencyId = currencyId,
            Offset = offset,
            Count = count
        };

        return _mediator.SendQueryAsync<GetSpendingsQuery, GetSpendingsResponseItem[]>(
            query,
            cancellationToken);
    }
    
    [HttpPost("delete")]
    public Task Delete([FromBody] DeleteSpendingRequest request, CancellationToken cancellationToken)
    {
        var command = new DeleteSpendingCommand
        {
            Id = request.Id,
            ActionSource = ActionSource.Api
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("update")]
    public Task Delete([FromBody] UpdateSpendingRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSpendingCommand
        {
            Id = request.Id,
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            CurrencyId = request.CurrencyId,
            ActionSource = ActionSource.Api
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }

    [HttpPost("add-to-exist-category")]
    public Task AddToExistCategory(
        [FromBody] AddSpendingToExistCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddSpendingToExistCategoryCommand
        {
            UserId = GetCurrentUserId(),
            SpendingId = request.SpendingId,
            CategoryId = request.CategoryId
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("add-to-new-category")]
    public Task AddToNewCategory(
        [FromBody] AddSpendingToNewCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddSpendingToNewCategoryCommand
        {
            UserId = GetCurrentUserId(),
            SpendingId = request.SpendingId,
            NewCategoryTitle = request.NewCategoryTitle
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("remove-from-category")]
    public Task RemoveSpendingFromCategory(
        [FromBody] RemoveSpendingFromCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveSpendingFromCategoryCommand
        {
            SpendingId = request.SpendingId,
            CategoryId = request.CategoryId
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
}
