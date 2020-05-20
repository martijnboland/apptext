using System.Linq;
using System.Threading.Tasks;
using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Storage;
using AppText.Translations.ViewModels;
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
        [FormatFilter]
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
            var collections = (await _contentStore.GetContentCollections(contentCollectionQuery)).Where(c => c.ContentType.Id == translationContentType.Id);
            
            var result = new TranslationResult
            {
                Language = language,
                Collection = collection
            };

            foreach (var contentCollection in collections)
            {
                var contentItems = await _contentStore.GetContentItems(new ContentItemQuery { AppId = appId, CollectionId = contentCollection.Id });
                foreach (var contentItem in contentItems)
                {
                    var entry = new TranslationResultEntry
                    {
                        Key = contentItem.ContentKey,
                        Collection = contentCollection.Name
                    };
                    entry.Value = entry.Key; // set translation to key as default
                    if (contentItem.Content.ContainsKey(Constants.TranslationTextFieldName))
                    {
                        var contentItemFieldValue = JObject.FromObject(contentItem.Content[Constants.TranslationTextFieldName]);
                        if (contentItemFieldValue != null)
                        {
                            var jToken = contentItemFieldValue.GetValue(language);
                            if (jToken != null)
                            {
                                entry.Value = jToken.ToString();
                            }
                        }
                    }
                    result.Entries.Add(entry);
                }
            }
            return Ok(result);
        }
    }
}