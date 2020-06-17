using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace AppText.Localization
{
    public class AppTextStringLocalizer : IStringLocalizer
    {
        private readonly string _baseName;
        private readonly AppTextBridge _appTextBridge;

        public AppTextStringLocalizer(string baseName, AppTextBridge appTextBridge)
        {
            _baseName = baseName;
            _appTextBridge = appTextBridge;
        }

        public LocalizedString this[string name]
        {
            get
            {
                return new LocalizedString(name, "LocalizedString");
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return new LocalizedString(name, "LocalizedStringWithParams");
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return new List<LocalizedString>();
        }

        public IStringLocalizer WithCulture(CultureInfo culture) => this;
    }
}
