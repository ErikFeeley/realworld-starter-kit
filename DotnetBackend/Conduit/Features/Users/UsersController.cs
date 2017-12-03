using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Conduit.Features.Users
{
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command)
        {
            var result = await _mediator.Send(command);

            return new OkObjectResult(result);
        }
    }
}
