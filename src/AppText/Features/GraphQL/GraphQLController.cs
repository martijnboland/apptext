using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AppText.Features.GraphQL.Graphiql;
using AppText.Features.GraphQL;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        private readonly IDocumentWriter _writer;
        private readonly SchemaResolver _schemaResolver;

        public GraphQLController(IDocumentExecuter executer, IDocumentWriter writer, SchemaResolver schemaResolver)
        {
            _executer = executer;
            _writer = writer;
            _schemaResolver = schemaResolver;
        }

        [HttpGet]
        public async Task<ActionResult> ExecuteGet(string appId)
        {
            var gqlRequest = new GraphQLRequest();
            ExtractGraphQLRequestFromQueryString(Request.Query, gqlRequest);

            return await ExecuteInternal(gqlRequest, appId);
        }

        [HttpPost]
        public async Task<ActionResult> ExecutePost(string appId)
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

        [HttpGet("graphiql")]
        public IActionResult GetGraphiql(string appId)
        {
            var graphQLUrl = Url.Action(nameof(this.ExecutePost), new { appId });

            var pageModel = new GraphiQLPageModel(graphQLUrl);

            var content = pageModel.Render();

            return Content(content, "text/html");
        }

        private async Task<ActionResult> ExecuteInternal(GraphQLRequest gqlRequest, string appId)
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
                options.Inputs = gqlRequest.GetInputs();

                options.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
                options.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                options.ExposeExceptions = true;

            }).ConfigureAwait(false);

            var json = await _writer.WriteToStringAsync(result);

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

        private static void ExtractGraphQLRequestFromQueryString(IQueryCollection qs, GraphQLRequest gqlRequest)
        {
            gqlRequest.Query = qs.TryGetValue(GraphQLRequest.QueryKey, out var queryValues) ? queryValues[0] : null;
            gqlRequest.Variables = qs.TryGetValue(GraphQLRequest.VariablesKey, out var variablesValues) ? JObject.Parse(variablesValues[0]) : null;
            gqlRequest.OperationName = qs.TryGetValue(GraphQLRequest.OperationNameKey, out var operationNameValues) ? operationNameValues[0] : null;
        }

        private static void ExtractGraphQLRequestFromPostBody(IFormCollection fc, GraphQLRequest gqlRequest)
        {
            gqlRequest.Query = fc.TryGetValue(GraphQLRequest.QueryKey, out var queryValues) ? queryValues[0] : null;
            gqlRequest.Variables = fc.TryGetValue(GraphQLRequest.VariablesKey, out var variablesValue) ? JObject.Parse(variablesValue[0]) : null;
            gqlRequest.OperationName = fc.TryGetValue(GraphQLRequest.OperationNameKey, out var operationNameValues) ? operationNameValues[0] : null;
        }
    }
}