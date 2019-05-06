using AppText.Shared.Infrastructure.Mvc;
using AppText.Features.ContentManagement;
using AppText.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Features.ContentDefinition
{
    [Route("{appId}/contenttypes")]
    public class ContentTypesController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentTypesController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string appId, [FromQuery]ContentTypeQuery query)
        {
            query.AppId = appId;
            return Ok(await _dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string appId, string id)
        {
            var result = await _dispatcher.ExecuteQuery(new ContentTypeQuery { Id = id, AppId = appId });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string appId, [FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(appId, contentType);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentType.Id, contentType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string appId, string id, [FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(appId, contentType);
            command.ContentType.Id = id;
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string appId, string id)
        {
            var result = await _dispatcher.ExecuteCommand(new DeleteContentTypeCommand(appId, id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}
