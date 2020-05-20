using AppText.Translations.ViewModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Net.Mime;
using System.Resources.NetStandard;
using System.Text;
using System.Threading.Tasks;

namespace AppText.Translations.Formatters
{
    /// <summary>
    /// Formatter that converts a TranslationResult into a .NET-compatible resx file.
    /// </summary>
    public class TranslationResultResxFormatter : TextOutputFormatter
    {
        public TranslationResultResxFormatter()
        {
            SupportedMediaTypes.Add("text/microsoft-resx");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override void WriteResponseHeaders(OutputFormatterWriteContext context)
        {
            context.ContentType = "text/microsoft-resx";
            var translationResult = (TranslationResult)context.Object;
            var fileName = String.IsNullOrEmpty(translationResult.Collection)
                ? $"translations.{translationResult.Language}.resx"
                : $"{translationResult.Collection}.{translationResult.Language}.resx";
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
            var memoryStream = new MemoryStream();
            var resxWriter = new ResXResourceWriter(memoryStream);
            var translationResult = (TranslationResult)context.Object;
            translationResult.Entries.ForEach(entry =>
            {
                // Prefix key with collection when no collection is set for the request.
                var key = String.IsNullOrEmpty(translationResult.Collection) ? $"{entry.Collection}:{entry.Key}" : entry.Key; 
                resxWriter.AddResource(key, entry.Value);
            });
            resxWriter.Generate();

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
