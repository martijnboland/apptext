using Microsoft.Extensions.Localization;
using System;

namespace AppText.Localization
{
    public class AppTextStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly AppTextBridge _appTextBridge;

        public AppTextStringLocalizerFactory(AppTextBridge appTextBridge)
        {
            _appTextBridge = appTextBridge;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new AppTextStringLocalizer(baseName, _appTextBridge);
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new AppTextStringLocalizer(resourceSource.Name, _appTextBridge);
        }
    }
}
