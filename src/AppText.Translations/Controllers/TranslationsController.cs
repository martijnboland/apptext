using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AppText.Translations.Controllers
{
    [Route("{appId}/translations")]
    [ApiController]
    public class TranslationsController : ControllerBase
    {
        private readonly IContentStore _contentStore;
        private readonly IContentDefinitionStore _contentDefinitionStore;

        public TranslationsController(IContentDefinitionStore contentDefinitionStore, IContentStore contentStore)
        {
            _contentDefinitionStore = contentDefinitionStore;
            _contentStore = contentStore;
        }

        /// <summary>
        /// Find all content with system content type 'Translation' and the given language. Optionally restrict to the given collection.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="collection"></param>
        /// <returns>Translations as key-value pairs, ordered by key.</returns>
        [HttpGet("{language}/{collection?}")]
        public async Task<IActionResult> Get(string appId, string language, string collection = null)
        {
            // Get all collections with the Translation content type and then return all content as key-value pairs, ordered by key.
            var translationContentType = (await _contentDefinitionStore.GetContentTypes(new ContentTypeQuery { AppId = null, Name = Constants.TranslationContentType, IncludeGlobalContentTypes = true })).FirstOrDefault();
            if (translationContentType == null)
            {
                return NotFound($"The {Constants.TranslationContentType} content type could not be found");
            }
            var contentCollectionQuery = new ContentCollectionQuery
            {
                AppId = appId
            };
            if (! string.IsNullOrEmpty(collection))
            {
                contentCollectionQuery.Name = collection;
            }
            var collections = await _contentStore.GetContentCollections(contentCollectionQuery);
            var result = new SortedDictionary<string, string>();
            foreach (var contentCollection in collections)
            {
                var contentItems = await _contentStore.GetContentItems(new ContentItemQuery { AppId = appId, CollectionId = contentCollection.Id });
                foreach (var contentItem in contentItems)
                {
                    // Prefix content key with collection name when no collection is given to prevent duplicate key errors.
                    var key = String.IsNullOrEmpty(collection) ? $"{contentCollection.Name}:{contentItem.ContentKey}" : contentItem.ContentKey;
                    string value = null;
                    if (contentItem.Content.ContainsKey(Constants.TranslationTextFieldName))
                    {
                        var contentItemFieldValue = contentItem.Content[Constants.TranslationTextFieldName] as JObject;
                        if (contentItemFieldValue != null)
                        {
                            var jToken = contentItemFieldValue.GetValue(language);
                            if (jToken != null)
                            {
                                value = jToken.ToString();
                            }
                        }
                    }
                    result.Add(key, value);
                }
            }
            return Ok(result);
        }
    }
}