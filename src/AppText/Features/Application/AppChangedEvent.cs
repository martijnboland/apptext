using AppText.Shared.Commands;

namespace AppText.Features.Application
{
    public class AppChangedEvent : IEvent
    {
        public string AppId { get; set; }
        public string DisplayName { get; set; }
        public string[] Languages { get; set; }
        public string DefaultLanguage { get; set; }
    }
}
