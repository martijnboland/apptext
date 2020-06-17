using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;

namespace AppText.Localization
{
    public class AppTextHtmlLocalizer : HtmlLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public AppTextHtmlLocalizer(IStringLocalizer localizer) : base(localizer)
        {
            _localizer = localizer;
        }

        public override LocalizedHtmlString this[string name] => ToHtmlString(_localizer[name]);

        public override LocalizedHtmlString this[string name, params object[] arguments] => ToHtmlString(_localizer[name], arguments);
    }
}
