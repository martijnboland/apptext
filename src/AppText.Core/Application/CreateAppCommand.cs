using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppText.Core.Application
{
    public class CreateAppCommand : ICommand
    {
        [StringLength(20)]
        public string PublicId { get; set; }
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; }
        public string[] Languages { get; set; }
        public string DefaultLanguage { get; set; }

        public App CreateApp()
        {
            return new App
            {
                PublicId = this.PublicId,
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
                await _store.AddApp(app);
                result.SetResultData(app);
            }
            return result;
        }
    }
}
