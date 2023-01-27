namespace AppText.AdminApp.Infrastructure.Vite
{
    public class ManifestItem
    {
        public string File { get; set; }
        public string Src { get; set; }
        public bool IsEntry { get; set; }
        public string[] Imports { get; set; }
        public string[] DynamicImports { get; set; }
        public string[] Css { get; set; }
        public string[] Assets { get; set; }
    }
}
