using AppText.Api.Infrastructure;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("collections")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public CollectionsController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]ContentCollectionQuery query)
        {
            return Ok(_dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _dispatcher.ExecuteQuery(new ContentCollectionQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public IActionResult Create([FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(contentCollection);
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentCollection.Id, contentCollection);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(contentCollection);
            command.ContentCollection.Id = id;
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var result = _dispatcher.ExecuteCommand(new DeleteContentCollectionCommand(id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}