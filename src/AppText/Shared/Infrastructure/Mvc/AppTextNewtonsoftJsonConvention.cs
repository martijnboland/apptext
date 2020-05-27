using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;
using System.Reflection;

namespace AppText.Shared.Infrastructure.Mvc
{
    public class AppTextNewtonsoftJsonConvention : IControllerModelConvention
    {
        private readonly Assembly _assembly;

        public AppTextNewtonsoftJsonConvention(Assembly assembly)
        {
            _assembly = assembly;
        }

        public void Apply(ControllerModel controller)
        {
            if (ShouldApplyConvention(controller))
            {
                var formatterAttribute = new NewtonsoftJsonFormatterAttribute();
                // The attribute itself also implements IControllerModelConvention so we call that one as well.
                // This way, the NewtonsoftJsonBodyModelBinder will be properly connected to the controller actions.
                formatterAttribute.Apply(controller);
                controller.Filters.Add(formatterAttribute);
            }
        }

        private bool ShouldApplyConvention(ControllerModel controller)
        {
            // Only apply NewtonsoftJsonFormatter filter on controllers from the given assembly or our own assembly when no specific assembly is set and without existing attribute.
            var assemblyType = this._assembly ?? this.GetType().Assembly;

            return controller.ControllerType.Assembly == assemblyType &&
                !controller.Attributes.Any(x => x.GetType() == typeof(NewtonsoftJsonFormatterAttribute));
        }
    }
}
