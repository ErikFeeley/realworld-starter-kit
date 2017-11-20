using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Tests
{
    [Route("api/[controller]")]
    public class TestsController
    {
        public IActionResult Index() => new OkObjectResult("hi");
    }
}
