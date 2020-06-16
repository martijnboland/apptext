using System.Collections.Generic;

namespace AppText.Features.Application
{
    public class AppInitializerOptions
    {
        public List<App> Apps { get; }

        public AppInitializerOptions()
        {
            this.Apps = new List<App>();
        }
    }
}
