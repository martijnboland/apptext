using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using AppText.Storage;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace AppText.Localization
{
    public class ContentItemChangedEventHandler : IEventHandler<ContentItemChangedEvent>
    {
        private readonly AppTextBridge _appTextBridge;
        private readonly AppTextLocalizationOptions _options;
        private readonly IContentStore _contentStore;

        public ContentItemChangedEventHandler(IContentStore contentStore, AppTextBridge appTextBridge, IOptions<AppTextLocalizationOptions> options)
        {
            _appTextBridge = appTextBridge;
            _options = options.Value;
            _contentStore = contentStore;
        }

        public async Task Handle(ContentItemChangedEvent publishedEvent)
        {
            // Clear cache of AppTextBridge when an item has changed
            if (publishedEvent.AppId == _options.AppId)
            {
                var collection = (await _contentStore.GetContentCollections(new ContentCollectionQuery { Id = publishedEvent.CollectionId })).FirstOrDefault();
                if (collection != null && collection.Name == _options.CollectionName)
                {
                    _appTextBridge.ClearCache();
                }
            }
        }
    }
}
