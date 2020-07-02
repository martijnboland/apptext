using AppText.Features.Application;
using AppText.Shared.Commands;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AppText.Localization
{
    public class AppChangedEventHandler : IEventHandler<AppChangedEvent>
    {
        private readonly AppTextBridge _appTextBridge;
        private readonly AppTextLocalizationOptions _options;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public AppChangedEventHandler(AppTextBridge appTextBridge, IOptions<AppTextLocalizationOptions> options, IHostApplicationLifetime hostApplicationLifetime)
        {
            _appTextBridge = appTextBridge;
            _options = options.Value;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public Task Handle(AppChangedEvent publishedEvent)
        {
            // Clear cache of AppTextBridge when the app has changed that contains the translations for Localization
            // because it might be possible that the default language has been changed or that a new language was added.
            if (publishedEvent.AppId == _options.AppId)
            {
                _appTextBridge.ClearCache();

                if (_options.RecycleHostAppAfterSavingApp)
                {
                    // HACK: the supported languages and default language are set via the RequestLocalizationOptions, but these are only set once
                    // at application startup. To reflect the new language settings from the saved app, the process needs to be restarted.
                    _hostApplicationLifetime.StopApplication();
                }
            }
            return Task.CompletedTask;
        }
    }
}
