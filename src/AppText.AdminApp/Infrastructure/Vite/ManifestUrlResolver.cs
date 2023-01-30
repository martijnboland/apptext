using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;

namespace AppText.AdminApp.Infrastructure.Vite
{
    public abstract class ManifestUrlResolver
    {
        private static readonly DateTimeOffset ManifestConfigCacheDuration = DateTimeOffset.Now.AddHours(1);
        protected const string ManifestConfigCacheKey = "VITE_MANIFEST_CONFIG";

        protected IDictionary<string, ManifestItem> Items { get; private set; }

        protected string BaseFolder { get; set; }

        public string GetPathFromManifest(string srcPath, Func<string, string> urlResolver)
        {
            if (Items == null)
            {
                // Try to load from Cache first. When not found in the cache, load manifest from source.
                var cache = MemoryCache.Default;
                if (! cache.Contains(ManifestConfigCacheKey))
                {
                    var manifestConfig = LoadManifestConfigFromManifestJson();
                    cache.Add(ManifestConfigCacheKey, manifestConfig, new CacheItemPolicy { AbsoluteExpiration =  ManifestConfigCacheDuration });
                }
                var cachedManifestConfig = cache[ManifestConfigCacheKey] as ManifestConfig;
                Items = cachedManifestConfig.Items;
                BaseFolder = cachedManifestConfig.BaseFolder;
            }
            if (srcPath.StartsWith(BaseFolder))
            {
                srcPath = srcPath.Replace(BaseFolder, string.Empty);
            }
            var filePath = Items[srcPath]?.File;
            if (! string.IsNullOrEmpty(filePath))
            {
                return urlResolver($"{BaseFolder}{filePath}");
            }
            else
            {
                throw new Exception($"Could not find file {srcPath} in manifest");
            }
        }

        protected abstract ManifestConfig LoadManifestConfigFromManifestJson();

        protected class ManifestConfig
        {
            public string BaseFolder { get; set; }
            public IDictionary<string, ManifestItem> Items { get; set; }
        }
    }

    public class AssemblyManifestResolver : ManifestUrlResolver
    {
        private readonly Assembly _assemblyWithEmbeddedManifest;

        public AssemblyManifestResolver(Assembly assemblyWithEmbeddedManifest)
        {
            _assemblyWithEmbeddedManifest = assemblyWithEmbeddedManifest;
        }

        protected override ManifestConfig LoadManifestConfigFromManifestJson()
        {
            // Try to load manifest from assembly
            var manifestConfig = new ManifestConfig();

            var embeddedProvider = new EmbeddedFileProvider(_assemblyWithEmbeddedManifest);
            var assemblyContents = embeddedProvider.GetDirectoryContents("");
            var manifestFileInfo = assemblyContents.Where(c => c.Name.EndsWith("manifest.json")).FirstOrDefault();
            if (manifestFileInfo != null)
            {
                // Manifest file found, load items
                using (var manifestStream = manifestFileInfo.CreateReadStream())
                using (var streamReader = new StreamReader(manifestStream))
                {
                    var manifestJson = streamReader.ReadToEnd();
                    manifestConfig.Items = JsonConvert.DeserializeObject<IDictionary<string, ManifestItem>>(manifestJson);
                }

                // Also resolve BaseFolder, based on the virtual path of the manifest file.
                // For example, if the manifest path is 'wwwroot.dist.manifest.json', the BaseFolder becomes '~/dist/'. 
                var manifestPathWithoutWwwrootAndFileName = manifestFileInfo.Name.Replace("wwwroot.", string.Empty).Replace("manifest.json", string.Empty);
                if (! string.IsNullOrEmpty(manifestPathWithoutWwwrootAndFileName))
                {
                    manifestConfig.BaseFolder = $"~/{manifestPathWithoutWwwrootAndFileName.Replace(".", "/")}";
                }
                else
                {
                    manifestConfig.BaseFolder = "~/";
                }
            }
            else
            {
                manifestConfig.Items = new Dictionary<string, ManifestItem>();
            }
            return manifestConfig;
        }
    }

    public class FileSystemManifestUrlResolver : ManifestUrlResolver
    {
        public FileSystemManifestUrlResolver(string contentRootPath)
        {
            throw new NotImplementedException("Not yet implemented");
        }

        protected override ManifestConfig LoadManifestConfigFromManifestJson()
        {
            var manifestConfig = new ManifestConfig();

            return manifestConfig;
        }
    }
}
