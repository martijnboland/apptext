using AppText.Core.Shared.Commands;
using AppText.Core.Storage;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AppText.Core.Application
{
    public class UpdateAppCommand : ICommand
    {
        public string Id { get; set; }
        [Required]
        [StringLength(100)]
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

        public UpdateAppCommandHandler(IApplicationStore store, AppValidator validator)
        {
            _store = store;
            _validator = validator;
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
            }

            return result;
        }
    }
}
