using AppText.Features.Application;
using AppText.Features.ContentManagement;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using TranslationConstants = AppText.Translations.Constants;

namespace AppText.Localization
{
    public class AppTextBridge
    {
        private const string AppCacheKey = "Translations_App";
        private const string CacheKeyPrefix = "Translations_";

        private readonly ILogger<AppTextBridge> _logger;
        private readonly AppTextLocalizationOptions _options;
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private ISet<string> _cacheKeys;

        public AppTextBridge(
            IServiceProvider serviceProvider,
            IMemoryCache cache,
            IOptions<AppTextLocalizationOptions> options,
            ILogger<AppTextBridge> logger
        )
        {
            _options = options.Value;
            _cache = cache;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _cacheKeys = new HashSet<string>();
        }

        public string GetTranslation(string baseName, string name, CultureInfo culture)
        {
            var translationsDictionary = GetTranslationsDictionary(culture.Name);
            var key = _options.PrefixContentKeys ? $"{baseName}{_options.PrefixSeparator}{name}" : name;
            if (translationsDictionary.TryGetValue(key, out string translation))
            {
                return translation;
            }
            // Translation not found, check if we might need to add an empty content item
            if (_options.CreateItemsWhenNotFound)
            {
                translationsDictionary.Add(key, null); // Add key, so creating a new item will only happen once
                AsyncHelper.RunSync(() => CreateEmptyContentItem(key));
            }
            return null;
        }

        public Dictionary<string, string> GetAllTranslations(CultureInfo culture)
        {
            return GetTranslationsDictionary(culture.Name);
        }

        public void ClearCache()
        {
            _logger.LogInformation("Clearing app cache...");
            _cache.Remove(AppCacheKey);

            _logger.LogInformation("Clearing translations cache...");

            foreach (var cacheKey in _cacheKeys)
            {
                _cache.Remove(cacheKey);
            }
        }

        private Dictionary<string, string> GetTranslationsDictionary(string culture)
        {
            // When culture is invariant, the name is an empty string. Use the default language in that case.
            if (culture == String.Empty)
            {
                var currentApp = _cache.GetOrCreate(AppCacheKey, c => new Lazy<App>(LoadApp, LazyThreadSafetyMode.ExecutionAndPublication)).Value;
                if (currentApp == null)
                {
                    throw new InvalidOperationException($"Can not get the translations because the AppText app {_options.AppId} is not found");
                }
                culture = currentApp.DefaultLanguage;
            }

            var cachedDictionary = _cache.GetOrCreate(CacheKeyPrefix + culture, c => new Lazy<Dictionary<string, string>>(() =>
            {
                _logger.LogInformation("Initializing translations dictionary for culture {0}", culture);
                var dictionary = new Dictionary<string, string>();
                LoadTranslationsIntoDictionary(culture, dictionary);
                if (! _cacheKeys.Contains(c.Key))
                {
                    _cacheKeys.Add(c.Key.ToString());
                }
                return dictionary;
            }, LazyThreadSafetyMode.ExecutionAndPublication));

            return cachedDictionary.Value;
        }

        private App LoadApp()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();
                var app = AsyncHelper.RunSync(() => applicationStore.GetApp(_options.AppId));
                return app;
            }
        }

        private void LoadTranslationsIntoDictionary(string culture, Dictionary<string, string> dictionary)
        {
            var appId = _options.AppId;
            using (var scope = _serviceProvider.CreateScope())
            {
                var contentStore = scope.ServiceProvider.GetRequiredService<IContentStore>();
                var collection = AsyncHelper.RunSync(() => contentStore.GetContentCollections(new ContentCollectionQuery { AppId = appId, Name = _options.CollectionName })).FirstOrDefault();
                if (collection == null)
                {
                    throw new Exception($"Can not load translations because the collection {_options.CollectionName} does not exist in app {appId}");
                }
                var contentItems = AsyncHelper.RunSync(() => contentStore.GetContentItems(new ContentItemQuery { AppId = appId, CollectionId = collection.Id }));
                foreach (var contentItem in contentItems)
                {
                    string translation = null;
                    if (contentItem.Content.ContainsKey(TranslationConstants.TranslationTextFieldName))
                    {
                        var contentItemFieldValue = JObject.FromObject(contentItem.Content[TranslationConstants.TranslationTextFieldName]);
                        if (contentItemFieldValue != null)
                        {
                            var jToken = contentItemFieldValue.GetValue(culture);
                            if (jToken != null)
                            {
                                translation = jToken.ToString();
                            }
                        }
                    }
                    dictionary.Add(contentItem.ContentKey, translation);
                }
            }
        }

        private async Task CreateEmptyContentItem(string key)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var appId = _options.AppId;

                // Service locate scoped components. It's not the most elegant thing, but it's the easiest solution.
                var contentStore = scope.ServiceProvider.GetRequiredService<IContentStore>();
                var applicationStore = scope.ServiceProvider.GetRequiredService<IApplicationStore>();
                var versioner = scope.ServiceProvider.GetRequiredService<IVersioner>();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();

                var contentItem = (await contentStore.GetContentItems(new ContentItemQuery { AppId = appId, ContentKey = key })).FirstOrDefault();
                if (contentItem == null)
                {
                    var initPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "apptext-localization") }));
                    var saveContentItemCommandHandler = new SaveContentItemCommandHandler(contentStore, versioner, new ContentItemValidator(contentStore, applicationStore), initPrincipal, dispatcher);
                    var collection = (await contentStore.GetContentCollections(new ContentCollectionQuery { AppId = appId, Name = _options.CollectionName })).FirstOrDefault();

                    await saveContentItemCommandHandler.Handle(new SaveContentItemCommand
                    {
                        AppId = appId,
                        CollectionId = collection.Id,
                        ContentKey = key,
                        Content = new Dictionary<string, object>()
                    });
                }
            }
        }
    }
}
