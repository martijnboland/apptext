using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AppText.Features.Application
{
    public class App
    {
        [Required(ErrorMessage = "Required")]
        [StringLength(20, ErrorMessage = "StringLength|{1}")]
        [RegularExpression(@"^[_a-z][_0-9a-z]*", ErrorMessage = "AppIdInvalid")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength|{1}")]
        public string DisplayName { get; set; }

        public string[] Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public bool IsSystemApp { get; set; }

        /// <summary>
        /// Removes invalid characters from the Id
        /// </summary>
        public void SanitizeAppId()
        {
            this.Id = SanitizeAppId(this.Id);
        }

        /// <summary>
        /// Removes all invalid characters from the given appId
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string SanitizeAppId(string appId)
        {
            // appId must be lowercase and can only contain ASCII characters, numbers and underscores
            appId = appId.ToLowerInvariant();
            appId = Regex.Replace(appId, @"[^\w]*", String.Empty);
            return appId;
        }
    }
}