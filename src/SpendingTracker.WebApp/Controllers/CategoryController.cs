using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Category.AddExistCategoryAsChildren.Contracts;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsParent.Contracts;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;
using SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;
using SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;
using SpendingTracker.Application.Handlers.Category.RemoveChildCategoryFromParent.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.Domain;
using SpendingTracker.WebApp.Contracts.AddExistCategoryAsChildren;
using SpendingTracker.WebApp.Contracts.AddNewCategoryAsParent;
using SpendingTracker.WebApp.Contracts.CreateCategory;
using SpendingTracker.WebApp.Contracts.DeleteCategory;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/category")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class CategoryController : BaseController
{
    private readonly IMediator _mediator;
    
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    public Task Create(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand
        {
            UserId = GetCurrentUserId(),
            Title = request.Title
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("delete")]
    public Task Create(
        [FromBody] DeleteCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand
        {
            InitiatorId = GetCurrentUserId(),
            Id = request.Id
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("parent/add-as-new")]
    public Task AddNewCategoryAsParent(
        [FromBody] AddNewCategoryAsParentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddNewCategoryAsParentCommand
        {
            UserId = GetCurrentUserId(),
            ChildId = request.ChildId,
            NewParentTitle = request.NewParentTitle,
            ActionSource = ActionSource.Api
        };

        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("child/add-exist")]
    public Task AddExistCategoryAsChildren(
        [FromBody] AddExistCategoryAsChildrenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddExistCategoryAsChildrenCommand
        {
            UserId = GetCurrentUserId(),
            ParentId = request.ParentId,
            ChildId = request.ChildId,
            ActionSource = ActionSource.Api
        };
        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("remove-child-from-parent")]
    public Task RemoveChildCategoryFromParent(
        [FromBody] RemoveChildCategoryFromParentCommand command,
        CancellationToken cancellationToken)
    {
        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpGet("list")]
    [Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
    public Task<GetUserCategoriesResponseItem[]> GetUserCategories(CancellationToken cancellationToken)
    {
        var query = new GetUserCategoriesQuery
        {
            UserId = GetCurrentUserId()
        };

        return _mediator.SendQueryAsync<GetUserCategoriesQuery, GetUserCategoriesResponseItem[]>(
            query,
            cancellationToken);
    }
}