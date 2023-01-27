using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AppText.AdminApp.Infrastructure.Vite
{
    [HtmlTargetElement("script", Attributes = ViteSrcAttributeName)]
    public class ViteTagHelper : TagHelper
    {
        private const string ViteSrcAttributeName = "vite-src";
        private AssemblyManifestResolver _manifestUrlResolver;

        [HtmlAttributeName(ViteSrcAttributeName)]
        public string ViteSrc { get; set; }

        public ViteTagHelper()
        {
            _manifestUrlResolver = new AssemblyManifestResolver(typeof(ViteTagHelper).Assembly);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("src", _manifestUrlResolver.GetPathFromManifest(ViteSrc));
        }
    }
}
