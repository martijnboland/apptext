using AppText.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppText.Localization
{
    public class AppTextBridge
    {
        private readonly Func<IApplicationStore> _getApplicationStore;
        private readonly Func<IContentStore> _getContentStore;
        private readonly AppTextLocalizationOptions _options;

        public AppTextBridge(Func<IApplicationStore> getApplicationStore, Func<IContentStore> getContentStore, IOptions<AppTextLocalizationOptions> options)
        {
            _getApplicationStore = getApplicationStore;
            _getContentStore = getContentStore;
            _options = options.Value;
        }


    }
}
