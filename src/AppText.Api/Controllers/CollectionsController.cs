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
            if (result.IsSuccess)
            {
                return Created(contentCollection.Id, contentCollection);
            }
            else
            {
                return UnprocessableEntity(result);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentCollection contentItem)
        {
            var command = new SaveContentCollectionCommand(contentItem);
            command.ContentCollection.Id = id;
            _dispatcher.ExecuteCommand(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _dispatcher.ExecuteCommand(new DeleteContentCollectionCommand(id));
            return NoContent();
        }
    }
}