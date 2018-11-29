using AppText.Api.Infrastructure;
using AppText.Core.ContentDefinition;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Api.Controllers
{
    [Route("{appPublicId}/contenttypes")]
    [ApiController]
    public class ContentTypesController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentTypesController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string appPublicId, [FromQuery]ContentTypeQuery query)
        {
            query.AppPublicId = appPublicId;
            return Ok(await _dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await _dispatcher.ExecuteQuery(new ContentTypeQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string appPublicId, [FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(appPublicId, contentType);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentType.Id, contentType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string appPublicId, string id, [FromBody]ContentType contentType)
        {
            var command = new SaveContentTypeCommand(appPublicId, contentType);
            command.ContentType.Id = id;
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string appPublicId, string id)
        {
            var result = await _dispatcher.ExecuteCommand(new DeleteContentTypeCommand(appPublicId, id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}
