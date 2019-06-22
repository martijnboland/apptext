using AppText.Shared.Commands;
using AppText.Shared.Validation;
using AppText.Storage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppText.Features.Application
{
    public class CreateAppCommand : ICommand
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string[] Languages { get; set; }

        public string DefaultLanguage { get; set; }

        public App CreateApp()
        {
            return new App
            {
                Id = this.Id,
                DisplayName = this.DisplayName,
                Languages = this.Languages,
                DefaultLanguage = this.DefaultLanguage
            };
        }
    }

    public class CreateAppCommandHandler : ICommandHandler<CreateAppCommand>
    {
        private readonly IApplicationStore _store;
        private readonly IValidator<App> _validator;

        public CreateAppCommandHandler(IApplicationStore store, IValidator<App> validator)
        {
            _store = store;
            _validator = validator;
        }

        public async Task<CommandResult> Handle(CreateAppCommand command)
        {
            var result = new CommandResult();
            var app = command.CreateApp();
            if (! await _validator.IsValid(app))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                if (await _store.AppExists(app.Id))
                {
                    result.AddValidationError(new ValidationError
                    {
                        Name = "Id",
                        ErrorMessage = "AppText:IdAlreadyExists",
                        Parameters = new[] { app.Id }
                    });
                }
                else
                {
                    await _store.AddApp(app);
                    result.SetResultData(app);
                }
            }
            return result;
        }
    }
}
