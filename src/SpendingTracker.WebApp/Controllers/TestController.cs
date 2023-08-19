using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SpendingTracker.WebApp.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<Response> Get()
        {
            return new Response<string>("Ok");
        }
    }
}