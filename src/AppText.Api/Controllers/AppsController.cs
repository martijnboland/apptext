using AppText.Api.Infrastructure;
using AppText.Core.Application;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("apps")]
    [ApiController]
    public class AppsController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public AppsController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]AppQuery query)
        {
            return Ok(_dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _dispatcher.ExecuteQuery(new AppQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateAppCommand command)
        {
            var result = _dispatcher.ExecuteCommand(command);
            var id = (result.ResultData as App)?.Id;
            return this.HandleCreateCommandResult(result, id, result.ResultData);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]UpdateAppCommand command)
        {
            command.Id = id;
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var result = _dispatcher.ExecuteCommand(new DeleteAppCommand(id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}