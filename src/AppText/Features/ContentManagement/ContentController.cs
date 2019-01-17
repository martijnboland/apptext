using AppText.Shared.Infrastructure;
using AppText.Shared.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentManagement
{
    [Route("{appId}/content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string appId, [FromQuery]ContentItemQuery query)
        {
            query.AppId = appId;
            return Ok(await _dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await _dispatcher.ExecuteQuery(new ContentItemQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string appId, [FromBody]SaveContentItemCommand contentItemCommand)
        {
            contentItemCommand.AppId = appId;
            var result = await _dispatcher.ExecuteCommand(contentItemCommand);
            var id = (result.ResultData as ContentItem)?.Id;
            return this.HandleCreateCommandResult(result, id, result.ResultData);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string appId, string id, [FromBody]SaveContentItemCommand contentItemCommand)
        {
            contentItemCommand.AppId = appId;
            contentItemCommand.Id = id;
            var result = await _dispatcher.ExecuteCommand(contentItemCommand);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string appId, string id)
        {
            var command = new DeleteContentItemCommand(appId, id);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleDeleteCommandResult(result);
        }
    }
}
