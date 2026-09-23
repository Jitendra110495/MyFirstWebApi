using Microsoft.Extensions.Logging;
using Moq;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Tests;

public class WelcomeServiceTests
{
    [Fact]
    public void GetWelcomeMessage_ShouldReturnCorrectMessage()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<WelcomeService>>();
        var service = new WelcomeService(loggerMock.Object);

        // Act
        var result = service.GetWelcomeMessage();

        // Assert
        Assert.Equal(
            "Welcome to my first ASP.NET Core Web API!",
            result);
    }
}