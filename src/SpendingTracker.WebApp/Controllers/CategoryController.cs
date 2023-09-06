using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Category;
using SpendingTracker.Application.Handlers.Category.AddExistCategoriesAsChildrenCommand.Contracts;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;
using SpendingTracker.Application.Handlers.Category.CreateCategory;
using SpendingTracker.Application.Handlers.Category.CreateCategory.Contracts;
using SpendingTracker.Application.Handlers.Category.DeleteCategory.Contracts;
using SpendingTracker.Application.Handlers.Category.GetUserCategories.Contracts;
using SpendingTracker.BearerTokenAuth;
using SpendingTracker.Common.Primitives;
using SpendingTracker.Dispatcher.Extensions;
using SpendingTracker.WebApp.Contracts.CreateCategory;
using SpendingTracker.WebApp.Contracts.DeleteCategory;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/category")]
[Authorize(AuthenticationSchemes = BearerAuth.SchemeName)]
public class CategoryController : Controller
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
    
    [HttpPost("child/add-as-new")]
    public Task AddChildCategory(
        [FromBody] AddNewCategoryAsChildCommand command,
        CancellationToken cancellationToken)
    {
        return _mediator.SendCommandAsync(command, cancellationToken);
    }
    
    [HttpPost("child/add-exist-list")]
    public Task AddExistCategoriesAsChildren(
        [FromBody] AddExistCategoriesAsChildrenCommand command,
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

    private UserKey GetCurrentUserId()
    {
        var userIdAsString = User.Claims.First(c => c.Type == "Id").Value;
        var result = UserKey.Parse(userIdAsString);

        return result;
    }
}