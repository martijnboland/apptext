using AppText.Api.Infrastructure;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("{appPublicId}/content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get(string appPublicId, [FromQuery]ContentItemQuery query)
        {
            query.AppPublicId = appPublicId;
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
        public IActionResult Create(string appPublicId, [FromBody]SaveContentItemCommand contentItemCommand)
        {
            contentItemCommand.AppPublicId = appPublicId;
            var result = _dispatcher.ExecuteCommand(contentItemCommand);
            var id = (result.ResultData as ContentItem)?.Id;
            return this.HandleCreateCommandResult(result, id, result.ResultData);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string appPublicId, string id, [FromBody]SaveContentItemCommand contentItemCommand)
        {
            contentItemCommand.AppPublicId = appPublicId;
            contentItemCommand.Id = id;
            var result = _dispatcher.ExecuteCommand(contentItemCommand);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string appPublicId, string id)
        {
            var command = new DeleteContentItemCommand(appPublicId, id);
            var result = _dispatcher.ExecuteCommand(command);
            return this.HandleDeleteCommandResult(result);
        }
    }
}
