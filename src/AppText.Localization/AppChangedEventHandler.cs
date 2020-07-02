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
            if (publishedEvent.AppId == _options.AppId)
            {
                _appTextBridge.ClearCache();
                if (_options.RecycleHostAppAfterSavingApp)
                {
                    _hostApplicationLifetime.StopApplication();
                }
            }
            return Task.CompletedTask;
        }
    }
}
