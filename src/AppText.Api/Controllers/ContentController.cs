using AppText.Core.ContentManagement;
using AppText.Core.Shared.Commands;
using AppText.Core.Shared.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppText.Api.Controllers
{
    [Route("content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ICommandHandler<SaveContentItemCommand> _saveContentCommandHandler;
        private readonly IQueryHandler<ContentItemQuery, ContentItem[]> _contentItemQueryHandler;

        public ContentController(
            IQueryHandler<ContentItemQuery, ContentItem[]> contentItemQueryHandler, 
            ICommandHandler<SaveContentItemCommand> saveContentCommandHandler)
        {
            _saveContentCommandHandler = saveContentCommandHandler;
            _contentItemQueryHandler = contentItemQueryHandler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]ContentItemQuery query)
        {
            return Ok(_contentItemQueryHandler.Handle(query));
        }

        [HttpGet("{id}")]
        public IActionResult GetOne(string id)
        {
            var result = _contentItemQueryHandler.Handle(new ContentItemQuery { Id = id });
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
            _saveContentCommandHandler.Handle(command);
            return Created(contentItem.Id, contentItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody]ContentItem contentItem)
        {
            var command = new SaveContentItemCommand(contentItem);
            command.ContentItem.Id = id;
            _saveContentCommandHandler.Handle(command);
            return Ok();
        }
    }
}
