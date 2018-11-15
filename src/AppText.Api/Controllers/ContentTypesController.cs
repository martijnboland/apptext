using AppText.Api.Infrastructure;
using AppText.Core.ContentDefinition;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("contenttypes")]
    [ApiController]
    public class ContentTypesController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentTypesController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]ContentTypeQuery query)
        {
            return Ok(_dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _dispatcher.ExecuteQuery(new ContentTypeQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public IActionResult Create([FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(contentType);
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentType.Id, contentType);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(contentType);
            command.ContentType.Id = id;
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var result = _dispatcher.ExecuteCommand(new DeleteContentTypeCommand(id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}
