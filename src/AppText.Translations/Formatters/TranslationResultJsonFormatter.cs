using AppText.Translations.ViewModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppText.Translations.Formatters
{
    /// <summary>
    /// Formatter that converts a TranslationResult instance into i18next-compatible JSON.
    /// </summary>
    public class TranslationResultJsonFormatter : TextOutputFormatter
    {
        public TranslationResultJsonFormatter()
        {
            SupportedMediaTypes.Add("application/json");
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            context.ContentType = "application/json";

            var translationResult = (TranslationResult)context.Object;
            var outputDictionary = translationResult.Entries.ToDictionary(
                e => String.IsNullOrEmpty(translationResult.Collection) ? $"{e.Collection}:{e.Key}" : e.Key,
                e => e.Value);
            await JsonSerializer.SerializeAsync(context.HttpContext.Response.Body, outputDictionary, typeof(IDictionary<string, string>));
        }

        protected override bool CanWriteType(Type type)
        {
            var canWriteType = type == typeof(TranslationResult);
            return canWriteType;
        }
    }
}
