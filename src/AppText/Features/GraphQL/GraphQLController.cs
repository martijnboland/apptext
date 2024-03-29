﻿using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AppText.Features.GraphQL.Graphiql;
using AppText.Features.GraphQL;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AppText.Shared.Infrastructure.Security.ApiKey;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace AppText.Features.Controllers
{
    [Route("{appId}/graphql")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private const string JsonContentType = "application/json";
        private const string GraphQLContentType = "application/graphql";
        private const string FormUrlEncodedContentType = "application/x-www-form-urlencoded";

        private readonly IDocumentExecuter _executer;
        private readonly IGraphQLTextSerializer _serializer;
        private readonly SchemaResolver _schemaResolver;

        public GraphQLController(IDocumentExecuter executer, IGraphQLTextSerializer serializer, SchemaResolver schemaResolver)
        {
            _executer = executer;
            _serializer = serializer;
            _schemaResolver = schemaResolver;
        }

        [HttpGet]
        public async Task<IActionResult> ExecuteGet(string appId)
        {
            var gqlRequest = new GraphQLRequest();
            ExtractGraphQLRequestFromQueryString(Request.Query, gqlRequest);

            return await ExecuteInternal(gqlRequest, appId);
        }

        [HttpGet("public")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
        public Task<IActionResult> ExecuteGetWithApiKey(string appId)
        {
            return ExecuteGet(appId);
        }

        [HttpPost]
        public async Task<IActionResult> ExecutePost(string appId)
        {
            var gqlRequest = new GraphQLRequest();
            if (!MediaTypeHeaderValue.TryParse(Request.ContentType, out var mediaTypeHeader))
            {
                return BadRequest($"Invalid 'Content-Type' header: value '{Request.ContentType}' could not be parsed.");
            }

            switch (mediaTypeHeader.MediaType)
            {
                case JsonContentType:
                    gqlRequest = await Deserialize<GraphQLRequest>(Request.Body);
                    break;
                case GraphQLContentType:
                    gqlRequest.Query = await ReadAsStringAsync(Request.Body);
                    break;
                case FormUrlEncodedContentType:
                    var formCollection = await Request.ReadFormAsync();
                    ExtractGraphQLRequestFromPostBody(formCollection, gqlRequest);
                    break;
                default:
                    return BadRequest($"Invalid 'Content-Type' header: non-supported media type. Must be of '{JsonContentType}', '{GraphQLContentType}', or '{FormUrlEncodedContentType}'. See: http://graphql.org/learn/serving-over-http/.");
            }

            return await ExecuteInternal(gqlRequest, appId);
        }

        [HttpPost("public")]
        [Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
        public Task<IActionResult> ExecutePostWithApiKey(string appId)
        {
            return ExecutePost(appId);
        }

        [HttpGet("graphiql")]
        public IActionResult GetGraphiql(string appId)
        {
            var graphQLUrl = Url.Action(nameof(this.ExecutePost), new { appId });

            var pageModel = new GraphiQLPageModel(graphQLUrl);

            var content = pageModel.Render();

            return Content(content, "text/html");
        }

        private async Task<IActionResult> ExecuteInternal(GraphQLRequest gqlRequest, string appId)
        {
            var schema = await _schemaResolver.Resolve(appId);
            if (schema == null)
            {
                return BadRequest($"No schema for {appId} could be resolved");
            }


            var result = await _executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Query = gqlRequest.Query;
                options.OperationName = gqlRequest.OperationName;
                options.Variables = gqlRequest.Variables;
                options.ThrowOnUnhandledException = true;

            }).ConfigureAwait(false);

            var json = _serializer.Serialize(result);

            var actionResult = new ContentResult
            {
                ContentType = JsonContentType,
                Content = json,
                StatusCode = result.Errors == null || result.Errors.Count == 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest
            };

            return actionResult;
        }

        private async static Task<T> Deserialize<T>(Stream s)
        {
            using (var reader = new StreamReader(s))
            using (var jsonReader = new JsonTextReader(reader))
            {
                var jObject = await JObject.LoadAsync(jsonReader);
                return jObject.ToObject<T>();
            }
        }

        private static async Task<string> ReadAsStringAsync(Stream s)
        {
            using (var reader = new StreamReader(s))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private void ExtractGraphQLRequestFromQueryString(IQueryCollection qs, GraphQLRequest gqlRequest)
        {
            gqlRequest.Query = qs.TryGetValue(GraphQLRequest.QueryKey, out var queryValues) ? queryValues[0] : null;
            gqlRequest.Variables = qs.TryGetValue(GraphQLRequest.VariablesKey, out var variablesValues) ? _serializer.Deserialize<Inputs>(variablesValues[0]) : null;
            gqlRequest.OperationName = qs.TryGetValue(GraphQLRequest.OperationNameKey, out var operationNameValues) ? operationNameValues[0] : null;
        }

        private void ExtractGraphQLRequestFromPostBody(IFormCollection fc, GraphQLRequest gqlRequest)
        {
            gqlRequest.Query = fc.TryGetValue(GraphQLRequest.QueryKey, out var queryValues) ? queryValues[0] : null;
            gqlRequest.Variables = fc.TryGetValue(GraphQLRequest.VariablesKey, out var variablesValue) ? _serializer.Deserialize<Inputs>(variablesValue[0]) : null;
            gqlRequest.OperationName = fc.TryGetValue(GraphQLRequest.OperationNameKey, out var operationNameValues) ? operationNameValues[0] : null;
        }
    }
}