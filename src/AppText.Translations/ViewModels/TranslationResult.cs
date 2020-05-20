using System.Collections.Generic;

namespace AppText.Translations.ViewModels
{
    public class TranslationResult
    {
        public string Language { get; set; }

        public string Collection { get; set; }

        public List<TranslationResultEntry> Entries { get; set; }

        public TranslationResult()
        {
            this.Entries = new List<TranslationResultEntry>();
        }
    }

    public class TranslationResultEntry
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Collection { get; set; }
    }
}
