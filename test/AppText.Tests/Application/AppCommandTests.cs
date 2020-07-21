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
                Id = "test",
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

        [Theory]
        [InlineData("Test")]
        [InlineData("test-with-hyphen")]
        [InlineData("test&*^")]
        public async Task Create_app_with_invalid_characters_returns_validationerror(string appId)
        {
            // Arrange
            var command = new CreateAppCommand
            {
                Id = appId,
                DisplayName = "Test"
            };
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
                Id = "test",
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
                Id = "test",
                DisplayName = "A new app with existing id",
                Languages = new[] { "en" },
                DefaultLanguage = "en"
            };
            var applicationStore = Substitute.For<IApplicationStore>();
            applicationStore.AppExists("test").Returns(true);
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
                Id = "test",
                DisplayName = "Test",
                Languages = new[] { "en" },
                DefaultLanguage = "en"
            };
            var command = new UpdateAppCommand
            {
                Id = "test",
                DisplayName = "Changed display name",
                Languages = new[] { "en", "fr" },
                DefaultLanguage = "fr"
            };
            var applicationStore = Substitute.For<IApplicationStore>();
            applicationStore.GetApp("test").Returns(existingApp);
            var dispatcher = Substitute.For<IDispatcher>();
            var handler = new UpdateAppCommandHandler(applicationStore, new AppValidator(), dispatcher);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.Equal(ResultStatus.Success, result.Status);
            await dispatcher.Received(1).PublishEvent(Arg.Is<AppChangedEvent>(ev => ev.AppId == "test" && ev.DefaultLanguage == "fr"));
        }

        [Fact]
        public async Task Update_non_existing_app_returns_notfounderror()
        {
            var command = new UpdateAppCommand
            {
                Id = "nonexisting",
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
