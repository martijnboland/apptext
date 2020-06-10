using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppText.Shared.Validation;

namespace AppText.Features.Application
{
    public class AppValidator : Validator<App>
    {
        protected override Task ValidateCustom(App app)  
        {
            // Default language must exist in list of languages
            if (app.DefaultLanguage != null && ! app.Languages.Any(l => l == app.DefaultLanguage))
            {
                AddError(new ValidationError { Name = "Languages", ErrorMessage = "DefaultLanguageNotInLanguages", Parameters = new object[] { app.Languages, app.DefaultLanguage } } );
            }

            // Ensure that there is no content in this app for languages that are removed
            return Task.CompletedTask;
        }
    }
}
