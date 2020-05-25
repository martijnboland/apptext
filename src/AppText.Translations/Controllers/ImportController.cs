using AppText.Features.ContentDefinition;
using AppText.Features.ContentManagement;
using AppText.Shared.Commands;
using AppText.Shared.Queries;
using AppText.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources.NetStandard;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppText.Translations.Controllers
{
    [Route("{appId}/translations/import")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IQueryHandler<ContentTypeQuery, ContentType[]> _contentTypeQueryHandler;
        private readonly IQueryHandler<ContentCollectionQuery, ContentCollection[]> _contentCollectionQueryHandler;
        private readonly ICommandHandler<SaveContentCollectionCommand> _saveContentCollectionCommand;
        private readonly IQueryHandler<ContentItemQuery, ContentItem[]> _contentItemQueryHandler;
        private readonly ICommandHandler<SaveContentItemCommand> _saveContentItemCommand;

        public ImportController(
            IQueryHandler<ContentTypeQuery, ContentType[]> contentTypeQueryHandler,
            IQueryHandler<ContentCollectionQuery, ContentCollection[]> contentCollectionQueryHandler,
            ICommandHandler<SaveContentCollectionCommand> saveContentCollectionCommand,
            IQueryHandler<ContentItemQuery, ContentItem[]> contentItemQueryHandler,
            ICommandHandler<SaveContentItemCommand> saveContentItemCommand, IQueryHandler<ContentCollectionQuery, ContentCollection[]> contentcollectionQueryHandler)
        {
            _contentTypeQueryHandler = contentTypeQueryHandler;
            _contentCollectionQueryHandler = contentCollectionQueryHandler;
            _saveContentCollectionCommand = saveContentCollectionCommand;
            _contentItemQueryHandler = contentItemQueryHandler;
            _saveContentItemCommand = saveContentItemCommand;
        }

        [HttpPost("fromresx/{language}/{collection}")]
        public async Task<IActionResult> FromResx(string appId, string language, string collection, [FromForm]IFormFile resxFile)
        {
            var translationContentType = (await _contentTypeQueryHandler
                .Handle(new ContentTypeQuery { AppId = null, Name = Constants.TranslationContentType, IncludeGlobalContentTypes = true }))
                .FirstOrDefault();
            if (translationContentType == null)
            {
                return NotFound($"The {Constants.TranslationContentType} content type could not be found");
            }

            var contentCollection = (await _contentCollectionQueryHandler
                .Handle(new ContentCollectionQuery { AppId = appId, Name = collection }))
                .FirstOrDefault();
            if (contentCollection == null)
            {
                contentCollection = new ContentCollection { ContentType = translationContentType, Name = collection, ListDisplayField = Constants.TranslationTextFieldName };
                var newContentCollectionCommand = new SaveContentCollectionCommand(appId, contentCollection);
                await _saveContentCollectionCommand.Handle(newContentCollectionCommand);
            }

            // HACK: we need to change the reader and writer type names in the resHeaders to the ones of System.Resources.NetStandard.ResXResourceReader and -Writer
            var xdoc = XDocument.Load(resxFile.OpenReadStream());
            xdoc.Root
                .Elements("resheader")
                .First(rh => rh.Attribute("name").Value == "reader")
                .Element("value")
                .SetValue("System.Resources.NetStandard.ResXResourceReader, System.Resources.NetStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            xdoc.Root
                .Elements("resheader")
                .First(rh => rh.Attribute("name").Value == "writer")
                .Element("value")
                .SetValue("System.Resources.NetStandard.ResXResourceWriter, System.Resources.NetStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            var ms = new MemoryStream();
            xdoc.Save(ms);
            ms.Position = 0;
            var resxReader = new ResXResourceReader(ms);

            var translationsDictionary = new Dictionary<string, string>();
            foreach (DictionaryEntry entry in resxReader)
            {
                translationsDictionary.Add((string)entry.Key, (string)entry.Value);
            }
            await UpdateCollectionFromDictionary(appId, contentCollection.Id, language, translationsDictionary);
            
            return Ok();
        }

        private async Task UpdateCollectionFromDictionary(string appId, string collectionId, string language, Dictionary<string, string> dictionary)
        {
            foreach (var keyValuePair in dictionary)
            {
                var saveContentItemCommand = new SaveContentItemCommand
                {
                    AppId = appId,
                    CollectionId = collectionId,
                    LanguagesToValidate = new[] { language },
                    ContentKey = keyValuePair.Key
                };

                var contentItem = (await _contentItemQueryHandler
                    .Handle(new ContentItemQuery { AppId = appId, CollectionId = collectionId, ContentKey = keyValuePair.Key }))
                    .FirstOrDefault();

                if (contentItem != null)
                {
                    saveContentItemCommand.Id = contentItem.Id;
                    saveContentItemCommand.Version = contentItem.Version;
                    saveContentItemCommand.Content = contentItem.Content;
                }

                var contentFieldValue = contentItem != null
                    ? JObject.FromObject(contentItem.Content[Constants.TranslationTextFieldName])
                    : new JObject();
                contentFieldValue[language] = keyValuePair.Value;
                saveContentItemCommand.Content[Constants.TranslationTextFieldName] = contentFieldValue;

                await _saveContentItemCommand.Handle(saveContentItemCommand);
            }
        }
    }
}
