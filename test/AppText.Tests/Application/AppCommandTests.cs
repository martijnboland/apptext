using AppText.Features.Application;
using AppText.Storage;
using NSubstitute;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AppText.Tests.Application
{
    public class AppCommandTests
    {
        [Fact]
        public async Task CreateEmptyApp_WithoutId_IsNotAllowed()
        {
            // Arrange
            var handler = new CreateAppCommandHandler(Substitute.For<IApplicationStore>(), new AppValidator());

            // Act
            var result = await handler.Handle(new CreateAppCommand());

            // Assert
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Contains(result.ValidationErrors, err => err.Name == "Id");
        }
    }
}
