using System.IO;
using System.Reflection;
using System.Text;

namespace AppText.Features.GraphQL.Graphiql
{
    public class GraphiQLPageModel
    {

        private string graphiQLCSHtml;
        private readonly string _graphQLEndpoint;

        public GraphiQLPageModel(string graphQLEndpoint)
        {
            _graphQLEndpoint = graphQLEndpoint;
        }

        public string Render()
        {
            if (graphiQLCSHtml != null)
            {
                return graphiQLCSHtml;
            }
            var assembly = typeof(GraphiQLPageModel).GetTypeInfo().Assembly;
            using (var manifestResourceStream = assembly.GetManifestResourceStream("AppText.Features.GraphQL.GraphiQL.graphiql.cshtml"))
            {
                using (var streamReader = new StreamReader(manifestResourceStream))
                {
                    var builder = new StringBuilder(streamReader.ReadToEnd());
                    builder.Replace("@Model.GraphQLEndPoint", this._graphQLEndpoint);
                    graphiQLCSHtml = builder.ToString();
                    return this.Render();
                }
            }
        }
    }
}
