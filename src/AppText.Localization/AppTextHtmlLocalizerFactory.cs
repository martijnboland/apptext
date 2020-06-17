using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppText.Localization
{
    public class AppTextHtmlLocalizerFactory : IHtmlLocalizerFactory
    {
        private readonly IStringLocalizerFactory _stringLocalizerFactory;

        public AppTextHtmlLocalizerFactory(IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public IHtmlLocalizer Create(string baseName, string location)
        {
            // Strip location from baseName
            var index = 0;
            if (baseName.StartsWith(location, StringComparison.OrdinalIgnoreCase))
            {
                index = location.Length;
            }

            if (baseName.Length > index && baseName[index] == '.')
            {
                index += 1;
            }

            var relativeName = baseName.Substring(index);

            return new AppTextHtmlLocalizer(_stringLocalizerFactory.Create(relativeName, location));
        }

        public IHtmlLocalizer Create(Type resourceSource)
        {
            return new AppTextHtmlLocalizer(_stringLocalizerFactory.Create(resourceSource));
        }
    }
}
