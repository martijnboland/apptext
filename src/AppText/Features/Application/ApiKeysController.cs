using System.Threading.Tasks;
using AppText.Shared.Infrastructure;
using AppText.Shared.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace AppText.Features.Application
{
    [Route("{appId}/apikeys")]
    public class ApiKeysController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public ApiKeysController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> GetApiKeys(string appId)
        {
            var query = new ApiKeysQuery { AppId = appId };
            return Ok(await _dispatcher.ExecuteQuery(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateApiKey(string appId, [FromBody] CreateApiKeyCommand command)
        {
            command.AppId = appId;
            var result = await _dispatcher.ExecuteCommand(command);
            var resultData = result.ResultData as CreateApiKeyResultData;
            if (resultData != null)
            {
                var uri = $"{appId}/apikeys/{resultData.ApiKey.Id}";
                var payload = new { ApiKey = resultData.ReadableKey };
                return this.HandleCreateCommandResult(result, uri, payload);
            }
            else
            {
                return this.HandleCreateCommandResult(result, null, null);
            }
        }

        [HttpDelete("{apiKeyId}")]
        public async Task<IActionResult> DeleteApiKey(string appId, string apiKeyId)
        {
            var command = new DeleteApiKeyCommand(apiKeyId, appId);
            var result = await _dispatcher.ExecuteCommand(command);
            return this.HandleDeleteCommandResult(result);
        }
    }
}
