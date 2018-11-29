using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace AppText.Api.Infrastructure.Mvc
{
    /// <summary>
    /// This convention adds a route prefix to all AppText route attributes.
    /// See https://www.strathweb.com/2016/06/global-route-prefix-with-asp-net-core-mvc-revisited/ for the idea.
    /// </summary>
    public class AppTextRouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _appTextPrefixModel;

        public AppTextRouteConvention(string prefix)
        {
            if (! string.IsNullOrEmpty(prefix))
            {
                var routeTemplateProvider = new RouteAttribute(prefix);
                _appTextPrefixModel = new AttributeRouteModel(routeTemplateProvider);
            }
        }

        public void Apply(ApplicationModel application)
        {
            if (_appTextPrefixModel != null)
            {
                var assemblyType = this.GetType().Assembly;
                var appTextControllers = application.Controllers.Where(c => c.ControllerType.Assembly == assemblyType);
                foreach (var controller in appTextControllers)
                {
                    var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                    if (matchedSelectors.Any())
                    {
                        foreach (var selectorModel in matchedSelectors)
                        {
                            selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_appTextPrefixModel,
                                selectorModel.AttributeRouteModel);
                        }
                    }
                }
            }
        }
    }
}
