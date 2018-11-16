using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.ComponentModel.DataAnnotations;

namespace AppText.Core.Application
{
    public class CreateAppCommand : ICommand
    {
        [StringLength(20)]
        public string PublicIdentifier { get; set; }
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
        public string[] Languages { get; set; }
        public string DefaultLanguage { get; set; }

        public App CreateApp()
        {
            return new App
            {
                PublicIdentifier = this.PublicIdentifier,
                DisplayName = this.DisplayName,
                Languages = this.Languages,
                DefaultLanguage = this.DefaultLanguage
            };
        }
    }

    public class CreateAppCommandHandler : ICommandHandler<CreateAppCommand>
    {
        private readonly IApplicationStore _store;
        private readonly AppValidator _validator;

        public CreateAppCommandHandler(IApplicationStore store, AppValidator validator)
        {
            _store = store;
            _validator = validator;
        }

        public CommandResult Handle(CreateAppCommand command)
        {
            var result = new CommandResult();

            var app = command.CreateApp();

            if (!_validator.IsValid(app))
            {
                result.AddValidationErrors(_validator.Errors);
            }
            else
            {
                _store.AddApp(app);
                result.SetResultData(app);
            }
            return result;
        }
    }
}
