using AppText.Api.Infrastructure;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using AppText.Core.Shared.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]ContentItemQuery query)
        {
            return Ok(_dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _dispatcher.ExecuteQuery(new ContentItemQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public IActionResult Create([FromBody]ContentItem contentItem)
        {
            var command = new SaveContentItemCommand(contentItem);
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentItem.Id, contentItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentItem contentItem)
        {
            var command = new SaveContentItemCommand(contentItem);
            command.ContentItem.Id = id;
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result, contentItem);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var command = new DeleteContentItemCommand(id);
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleDeleteCommandResult(result);
        }
    }
}
