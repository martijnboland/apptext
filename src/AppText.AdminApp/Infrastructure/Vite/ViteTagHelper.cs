using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace AppText.AdminApp.Infrastructure.Vite
{
    [HtmlTargetElement("script", Attributes = $"{ViteSrcAttributeName}, {ViteAssetModeAttributeName}")]
    [HtmlTargetElement("link", Attributes = $"{ViteHrefAttributeName}, {ViteAssetModeAttributeName}")]
    public class ViteTagHelper : TagHelper
    {
        private const string ViteSrcAttributeName = "vite-src";
        private const string ViteHrefAttributeName = "vite-href";
        private const string ViteAssetModeAttributeName = "vite-asset-mode";
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IUrlHelperFactory _urlHelperFactory;

        [HtmlAttributeName(ViteSrcAttributeName)]
        public string ViteSrc { get; set; }

        [HtmlAttributeName(ViteHrefAttributeName)]
        public string ViteHref { get; set; }

        [HtmlAttributeName(ViteAssetModeAttributeName)]
        public AssetMode AssetMode { get; set; } = AssetMode.FileSystem;

        public Assembly Assembly { get; set; } = Assembly.GetExecutingAssembly();

        /// <summary>
        /// The <see cref="ViewContext"/>.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        public ViteTagHelper(IHostEnvironment hostEnvironment, IUrlHelperFactory urlHelperFactory)
        {
            _hostEnvironment = hostEnvironment;
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ManifestUrlResolver manifestUrlResolver = AssetMode == AssetMode.EmbeddedResource
                ? new AssemblyManifestResolver(Assembly)
                : new FileSystemManifestUrlResolver(_hostEnvironment.ContentRootPath);
            if (! string.IsNullOrEmpty(ViteSrc))
            {
                output.Attributes.Add("src", manifestUrlResolver.GetPathFromManifest(ViteSrc, _urlHelperFactory.GetUrlHelper(ViewContext).Content));
            }
            if (! string.IsNullOrEmpty(ViteHref))
            {
                output.Attributes.Add("href", manifestUrlResolver.GetPathFromManifest(ViteHref, _urlHelperFactory.GetUrlHelper(ViewContext).Content));
            }
        }
    }
}
