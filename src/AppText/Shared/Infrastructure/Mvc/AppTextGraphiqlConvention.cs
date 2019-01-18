using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AppText.Shared.Infrastructure.Mvc
{
    public class AppTextGraphiqlConvention : IActionModelConvention
    {
        private readonly bool _enableGraphiql;

        public AppTextGraphiqlConvention(bool enableGraphiql)
        {
            _enableGraphiql = enableGraphiql;
        }

        public void Apply(ActionModel action)
        {
            if (ShouldDisableGraphiql(action))
            {
                // Clear selectors, so the controller action cannot be found anymore
                action.Selectors.Clear();
            }
        }

        private bool ShouldDisableGraphiql(ActionModel action)
        {
            // Apply convention when Graphiql is NOT enabled
            return ! _enableGraphiql && action.ActionName == "GetGraphiql";
        }
    }
}
