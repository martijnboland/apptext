using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
                if (name == null)
                {
                    throw new ArgumentNullException(nameof(name));
                }

                var translation = GetTranslation(name, CultureInfo.CurrentUICulture);

                return new LocalizedString(name, translation ?? name, translation == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var translationFormat = GetTranslation(name, CultureInfo.CurrentUICulture);
                var formatted = translationFormat != null ? string.Format(translationFormat, arguments) : null;
                return new LocalizedString(name, formatted ?? name, translationFormat == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var localizedStrings = new List<LocalizedString>();
            var currentCulture = CultureInfo.CurrentUICulture;

            if (includeParentCultures)
            {
                do
                {
                    var translations = GetAllTranslations(currentCulture);
                    foreach(var translation in translations)
                    {
                        if (! localizedStrings.Any(l => l.Name == translation.Key))
                        {
                            localizedStrings.Add(new LocalizedString(translation.Key, translation.Value));
                        }
                    }
                    currentCulture = currentCulture.Parent;
                }
                while (currentCulture.Parent != currentCulture);
            }
            else
            {
                localizedStrings.AddRange(GetAllTranslations(currentCulture).Select(t => new LocalizedString(t.Key, t.Value)));
            }
            return localizedStrings;
        }

        public IStringLocalizer WithCulture(CultureInfo culture) => this;

        private string GetTranslation(string name, CultureInfo culture)
        {
            var text = _appTextBridge.GetTranslation(_baseName, name, culture);
            if (text == null)
            {
                var parentCulture = culture.Parent;
                if (parentCulture != culture)
                {
                    return GetTranslation(name, culture.Parent);
                }
                return null;
            }
            return text;
        }

        private Dictionary<string, string> GetAllTranslations(CultureInfo culture)
        {
            return _appTextBridge.GetAllTranslations(culture);
        }
    }
}
