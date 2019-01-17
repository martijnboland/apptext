using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Linq;

namespace AppText.Shared.Infrastructure.Mvc
{
    public class AppTextAuthorizationConvention : IActionModelConvention
    {
        private readonly bool _requireAuthenticatedUser;
        private readonly string _requiredAuthorizationPolicy;

        public AppTextAuthorizationConvention(bool requireAuthenticatedUser, string requiredAuthorizationPolicy)
        {
            _requireAuthenticatedUser = requireAuthenticatedUser;
            _requiredAuthorizationPolicy = requiredAuthorizationPolicy;
        }

        public void Apply(ActionModel action)
        {
            if (ShouldApplyConvention(action))
            {
                if (! String.IsNullOrEmpty(_requiredAuthorizationPolicy))
                {
                    action.Filters.Add(new AuthorizeFilter(_requiredAuthorizationPolicy));
                }
                if (_requireAuthenticatedUser)
                {
                    var requiredUserPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    action.Filters.Add(new AuthorizeFilter(requiredUserPolicy));
                }
            }
        }

        private bool ShouldApplyConvention(ActionModel action)
        {
            // Only apply Authorization filters on actions from our own assembly and without existing attributes.
            var assemblyType = this.GetType().Assembly;

            return action.Controller.ControllerType.Assembly == assemblyType &&
                !action.Attributes.Any(x => x.GetType() == typeof(AuthorizeAttribute)) &&
                !action.Attributes.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
