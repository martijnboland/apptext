using Microsoft.AspNetCore.Mvc;

namespace HostAppExample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok("hello");
        }
    }
}