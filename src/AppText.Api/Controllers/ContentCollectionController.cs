using AppText.Core.ContentManagement;
using AppText.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("collections")]
    [ApiController]
    public class ContentCollectionController : ControllerBase
    {
        private readonly Dispatcher _dispatcher;

        public ContentCollectionController(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]ContentCollectionQuery query)
        {
            return Ok(_dispatcher.ExecuteQuery<ContentCollectionQuery, ContentCollection[]>(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _dispatcher.ExecuteQuery<ContentCollectionQuery, ContentCollection[]>(new ContentCollectionQuery { Id = id });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result.First());
        }

        [HttpPost]
        public IActionResult Create([FromBody]ContentCollection contentItem)
        {
            var command = new SaveContentCollectionCommand(contentItem);
            _dispatcher.ExecuteCommand(command);
            return Created(contentItem.Id, contentItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentCollection contentItem)
        {
            var command = new SaveContentCollectionCommand(contentItem);
            command.ContentCollection.Id = id;
            _dispatcher.ExecuteCommand(command);
            return Ok();
        }
    }
}