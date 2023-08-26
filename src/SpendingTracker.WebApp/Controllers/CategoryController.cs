using MediatR;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Handlers.Category.AddNewCategoryAsChild.Contracts;
using SpendingTracker.Dispatcher.Extensions;

namespace SpendingTracker.WebApp.Controllers;

[ApiController]
[Route("api/v1/category")]
public class CategoryController
{
    private readonly IMediator _mediator;
    
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("child/add")]
    public Task AddChildCategory([FromBody] AddNewCategoryAsChildCommand command, CancellationToken cancellationToken)
    {
        return _mediator.SendCommandAsync(command, cancellationToken);
    }   
}