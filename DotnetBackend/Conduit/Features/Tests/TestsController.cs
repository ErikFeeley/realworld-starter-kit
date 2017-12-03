using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Tests
{
    [Authorize]
    [Route("api/[controller]")]
    public class TestsController
    {
        [HttpGet]
        public IActionResult Index() => new OkObjectResult("hi halp");
    }
}
