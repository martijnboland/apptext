using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace AppText.Features.Application
{
    public class ApiKey
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string AppId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [JsonIgnore]
        public string Key { get; set; }

        public static string HashKey(string key, string appId)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Hash key together with appId
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes($"{key}|{appId}"));

                // Convert byte array to a string   
                var builder = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
