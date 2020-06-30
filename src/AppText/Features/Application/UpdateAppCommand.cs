using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Shared.Validation;
using AppText.Storage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class UpdateAppCommand : ICommand
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string[] Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public void UpdateApp(App app)
        {
            app.DisplayName = this.DisplayName;
            app.Languages = this.Languages;
            app.DefaultLanguage = this.DefaultLanguage;
        }
    }

    public class UpdateAppCommandHandler : ICommandHandler<UpdateAppCommand>
    {
        private readonly IApplicationStore _store;
        private readonly AppValidator _validator;
        private readonly Dispatcher _dispatcher;

        public UpdateAppCommandHandler(IApplicationStore store, AppValidator validator, Dispatcher dispatcher)
        {
            _store = store;
            _validator = validator;
            _dispatcher = dispatcher;
        }

        public async Task<CommandResult> Handle(UpdateAppCommand command)
        {
            var result = new CommandResult();

            var app = await _store.GetApp(command.Id);
            if (app == null)
            {
                result.SetNotFound();
                return result;
            }

            command.UpdateApp(app);

            if (! await _validator.IsValid(app))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                await _store.UpdateApp(app);
                await _dispatcher.PublishEvent(new AppChangedEvent
                {
                    AppId = app.Id,
                    DisplayName = app.DisplayName,
                    Languages = app.Languages,
                    DefaultLanguage = app.DefaultLanguage
                });
            }

            return result;
        }
    }
}
