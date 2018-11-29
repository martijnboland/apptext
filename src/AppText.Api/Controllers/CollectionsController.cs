using AppText.Api.Infrastructure;
using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Api.Controllers
{
    [Route("{appPublicId}/collections")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public CollectionsController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string appPublicId, [FromQuery]ContentCollectionQuery query)
        {
            query.AppPublicId = appPublicId;
            return Ok(_dispatcher.ExecuteQuery(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string id)
        {
            var result = await _dispatcher.ExecuteQuery(new ContentCollectionQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(contentCollection);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentCollection.Id, contentCollection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(contentCollection);
            command.ContentCollection.Id = id;
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _dispatcher.ExecuteCommand(new DeleteContentCollectionCommand(id));
            return this.HandleDeleteCommandResult(result);
        }
    }
}