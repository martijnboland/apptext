using AppText.Features.Application;
using AppText.Shared.Commands;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AppText.Localization
{
    public class AppChangedEventHandler : IEventHandler<AppChangedEvent>
    {
        private readonly AppTextBridge _appTextBridge;
        private readonly AppTextLocalizationOptions _options;

        public AppChangedEventHandler(AppTextBridge appTextBridge, IOptions<AppTextLocalizationOptions> options)
        {
            _appTextBridge = appTextBridge;
            _options = options.Value;
        }

        public Task Handle(AppChangedEvent publishedEvent)
        {
            // Clear cache of AppTextBridge when the app has changed that contains the translations for Localization
            if (publishedEvent.AppId == _options.AppId)
            {
                _appTextBridge.ClearCache();
            }
            return Task.CompletedTask;
        }
    }
}
