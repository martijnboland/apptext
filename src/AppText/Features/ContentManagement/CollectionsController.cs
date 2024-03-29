﻿using AppText.Shared.Infrastructure.Mvc;
using AppText.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AppText.Shared.Infrastructure.Security.ApiKey;
using Microsoft.AspNetCore.Authorization;

namespace AppText.Features.ContentManagement
{
    [Route("{appId}/collections")]
    public class CollectionsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public CollectionsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string appId, [FromQuery]ContentCollectionQuery query)
        {
            query.AppId = appId;
            return Ok(await _dispatcher.ExecuteQuery(query));
        }

        [HttpGet("public")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
        public Task<IActionResult> GetWithApiKey(string appId, [FromQuery] ContentCollectionQuery query)
        {
            return Get(appId, query);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(string appId, string id)
        {
            var result = await _dispatcher.ExecuteQuery(new ContentCollectionQuery { Id = id, AppId = appId });
            if (result.Length == 0)
            {
                return NotFound();
            }
            return Ok(result[0]);
        }

        [HttpGet("public/{id}")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
        public Task<IActionResult> GetOnWithApiKey(string appId, string id)
        {
            return GetOne(appId, id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string appId, [FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(appId, contentCollection);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleCreateCommandResult(result, contentCollection.Id, contentCollection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string appId, string id, [FromBody]ContentCollection contentCollection)
        {
            var command = new SaveContentCollectionCommand(appId, contentCollection);
            command.ContentCollection.Id = id;
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleUpdateCommandResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, string appId, bool deleteItems = false)
        {
            var result = await _dispatcher.ExecuteCommand(new DeleteContentCollectionCommand(id, appId, deleteItems));
            return this.HandleDeleteCommandResult(result);
        }
    }
}