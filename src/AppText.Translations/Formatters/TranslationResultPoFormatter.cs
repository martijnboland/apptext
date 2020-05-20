using AppText.Translations.ViewModels;
using Karambolo.PO;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace AppText.Translations.Formatters
{
    /// <summary>
    /// Formatter that converts a TranslationResult into a GNU GetText compatible .po file
    /// </summary>
    public class TranslationResultPoFormatter : TextOutputFormatter
    {
        public TranslationResultPoFormatter()
        {
            SupportedMediaTypes.Add("text/x-gettext-translation");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/x-gettext-translation";

            var translationResult = (TranslationResult)context.Object;
            var fileName = String.IsNullOrEmpty(translationResult.Collection)
                ? $"{translationResult.Language}.po"
                : $"{translationResult.Collection}.{translationResult.Language}.po";
            var contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = true
            };
            context.HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            base.WriteResponseHeaders(context);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var translationResult = (TranslationResult)context.Object;
            var poCatalog = new POCatalog();
            poCatalog.Language = translationResult.Language;
            poCatalog.Encoding = "UTF-8";

            translationResult.Entries.ForEach(entry =>
            {
                var poKey = new POKey(entry.Key, contextId: entry.Collection);
                var poEntry  = new POSingularEntry(poKey);
                poEntry.Translation = entry.Value;
                poCatalog.Add(poEntry);
            });
            var poGenerator = new POGenerator();
            var memoryStream = new MemoryStream();
            poGenerator.Generate(memoryStream, poCatalog, Encoding.UTF8);

            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(context.HttpContext.Response.Body);
        }

        protected override bool CanWriteType(Type type)
        {
            var canWriteType = type == typeof(TranslationResult);
            return canWriteType;
        }
    }
}
