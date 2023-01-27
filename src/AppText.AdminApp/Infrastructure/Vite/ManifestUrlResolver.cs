using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AppText.AdminApp.Infrastructure.Vite
{
    public abstract class ManifestUrlResolver
    {
        protected IDictionary<string, ManifestItem> Items;

        public string GetPathFromManifest(string srcPath)
        {
            if (Items == null)
            {
                InitFromManifestJson(srcPath);
            }
            return Items[srcPath]?.File;
        }

        protected abstract void InitFromManifestJson(string srcPath);
    }

    public class AssemblyManifestResolver : ManifestUrlResolver
    {
        private readonly Assembly _assemblyWithEmbeddedManifest;

        public AssemblyManifestResolver(Assembly assemblyWithEmbeddedManifest)
        {
            _assemblyWithEmbeddedManifest = assemblyWithEmbeddedManifest;
        }

        protected override void InitFromManifestJson(string srcPath)
        {
            // Try to load manifest from assembly
            var embeddedProvider = new EmbeddedFileProvider(_assemblyWithEmbeddedManifest);
            var assemblyContents = embeddedProvider.GetDirectoryContents("");
            var manifestFileInfo = assemblyContents.Where(c => c.Name.EndsWith("manifest.json")).FirstOrDefault();
            if (manifestFileInfo != null)
            {
                using (var manifestStream = manifestFileInfo.CreateReadStream())
                using (var streamReader = new StreamReader(manifestStream))
                {
                    var manifestJson = streamReader.ReadToEnd();
                    Items = JsonConvert.DeserializeObject<IDictionary<string, ManifestItem>>(manifestJson);
                }
            }
            else
            {
                Items = new Dictionary<string, ManifestItem>();
            }
        }
    }
}
