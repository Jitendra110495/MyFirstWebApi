/*
namespace MyFirstWebApi.Services;

public class WelcomeService : IWelcomeService
{
    public string GetWelcomeMessage()
    {
        return "Welcome! This message came from a service.";
    }
}
*/
namespace MyFirstWebApi.Services;

public class WelcomeService : IWelcomeService
{
    private readonly ILogger<WelcomeService> _logger;

    public WelcomeService(ILogger<WelcomeService> logger)
    {
        _logger = logger;
    }

    public string GetWelcomeMessage()
    {
        _logger.LogInformation("Welcome message was requested");

        return "Welcome to my first ASP.NET Core Web API!";
    }
}