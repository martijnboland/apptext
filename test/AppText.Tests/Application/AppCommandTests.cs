using AppText.Features.Application;
using AppText.Shared.Commands;
using AppText.Shared.Infrastructure;
using AppText.Storage;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace AppText.Tests.Application
{
    public class AppCommandTests
    {
        [Fact]
        public async Task Create_valid_app_returns_no_errors()
        {
            // Arrange
            var command = new CreateAppCommand
            {
                Id = "Test",
                DisplayName = "Test",
                Languages = new [] { "en" },
                DefaultLanguage = "en"
            };
            var handler = new CreateAppCommandHandler(Substitute.For<IApplicationStore>(), new AppValidator());

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Equal(ResultStatus.Success, result.Status);
        }

        [Fact]
        public async Task Create_app_without_id_returns_validationerror()
        {
            // Arrange
            var command = new CreateAppCommand();
            var handler = new CreateAppCommandHandler(Substitute.For<IApplicationStore>(), new AppValidator());

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Contains(result.ValidationErrors, err => err.Name == "Id");
        }

        [Fact]
        public async Task Create_app_with_nonexisting_defaultlanguage_returns_validationerror()
        {
            // Arrange
            var command = new CreateAppCommand
            {
                Id = "Test",
                DisplayName = "Test",
                Languages = new [] { "en", "fr" },
                DefaultLanguage = "nl"
            };
            var handler = new CreateAppCommandHandler(Substitute.For<IApplicationStore>(), new AppValidator());

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Contains(result.ValidationErrors, err => err.ErrorMessage == "DefaultLanguageNotInLanguages");
        }

        [Fact]
        public async Task Create_app_with_existing_id_returns_validationerror()
        {
            // Arrange
            var command = new CreateAppCommand
            {
                Id = "Test",
                DisplayName = "A new app with existing id",
                Languages = new[] { "en" },
                DefaultLanguage = "en"
            };
            var applicationStore = Substitute.For<IApplicationStore>();
            applicationStore.AppExists("Test").Returns(true);
            var handler = new CreateAppCommandHandler(applicationStore, new AppValidator());

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Equal(ResultStatus.ValidationError, result.Status);
            Assert.Contains(result.ValidationErrors, err => err.ErrorMessage == "AppIdAlreadyExists");
        }

        [Fact]
        public async Task Update_valid_app_is_allowed_and_raises_event()
        {
            // Arrange
            var existingApp = new App
            {
                Id = "Test",
                DisplayName = "Test",
                Languages = new[] { "en" },
                DefaultLanguage = "en"
            };
            var command = new UpdateAppCommand
            {
                Id = "Test",
                DisplayName = "Changed display name",
                Languages = new[] { "en", "fr" },
                DefaultLanguage = "fr"
            };
            var applicationStore = Substitute.For<IApplicationStore>();
            applicationStore.GetApp("Test").Returns(existingApp);
            var dispatcher = Substitute.For<IDispatcher>();
            var handler = new UpdateAppCommandHandler(applicationStore, new AppValidator(), dispatcher);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Equal(ResultStatus.Success, result.Status);
            await dispatcher.Received(1).PublishEvent(Arg.Is<AppChangedEvent>(ev => ev.AppId == "Test" && ev.DefaultLanguage == "fr"));
        }

        [Fact]
        public async Task Update_non_existing_app_returns_notfounderror()
        {
            var command = new UpdateAppCommand
            {
                Id = "NonExisting",
                DisplayName = "Non-existing app",
                Languages = new[] { "en" },
                DefaultLanguage = "en"
            };
            var applicationStore = Substitute.For<IApplicationStore>();
            applicationStore.GetApp("NonExisting").Returns((App)null);
            var dispatcher = Substitute.For<IDispatcher>();
            var handler = new UpdateAppCommandHandler(applicationStore, new AppValidator(), dispatcher);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Equal(ResultStatus.NotFound, result.Status);
        }
    }
}
