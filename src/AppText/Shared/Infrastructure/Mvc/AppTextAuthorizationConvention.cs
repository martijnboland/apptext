using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using System;
using System.Linq;
using System.Reflection;

namespace AppText.Shared.Infrastructure.Mvc
{
    public class AppTextAuthorizationConvention : IActionModelConvention
    {
        private readonly bool _requireAuthenticatedUser;
        private readonly string _requiredAuthorizationPolicy;
        private readonly Assembly _assembly;

        public AppTextAuthorizationConvention(bool requireAuthenticatedUser, string requiredAuthorizationPolicy, Assembly assembly = null)
        {
            _requireAuthenticatedUser = requireAuthenticatedUser;
            _requiredAuthorizationPolicy = requiredAuthorizationPolicy;
            _assembly = assembly;
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
            // Only apply Authorization filters on actions from the given assembly our own assembly when no specific assembly is set and without existing attributes.
            var assemblyType = this._assembly ?? this.GetType().Assembly;

            return action.Controller.ControllerType.Assembly == assemblyType &&
                !action.Attributes.Any(x => x.GetType() == typeof(AuthorizeAttribute)) &&
                !action.Attributes.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
