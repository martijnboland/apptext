using GraphQL;
using Newtonsoft.Json;

namespace AppText.Features.GraphQL
{
    public class GraphQLRequest
    {
        public const string QueryKey = "query";
        public const string VariablesKey = "variables";
        public const string OperationNameKey = "operationName";

        [JsonProperty(QueryKey)]
        public string Query { get; set; }

        [JsonProperty(VariablesKey)]
        public Inputs Variables { get; set; }

        [JsonProperty(OperationNameKey)]
        public string OperationName { get; set; }
    }
}