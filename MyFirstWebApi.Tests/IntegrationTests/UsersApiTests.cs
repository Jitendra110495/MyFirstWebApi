using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MyFirstWebApi.Tests.IntegrationTests;

public class UsersApiTests
{
    [Fact]
    public async Task GetUsers_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_WithValidToken_ShouldReturnSuccess()
    {
    // Arrange
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var loginRequest = new
        {
            email = "test@example.com",
            password = "Test@123"
        };

        // Act
        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            loginRequest);

        var loginResult = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);

        var response = await client.GetAsync("/api/users");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminUsers_WithUserToken_ShouldReturnForbidden()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var loginRequest = new
        {
            email = "normal@example.com",
            password = "Test@123"
        };

        // Act
        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            loginRequest);

        var loginResult = await loginResponse.Content
        .ReadFromJsonAsync<LoginResponse>();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                loginResult!.Token);

        var response = await client.GetAsync("/api/users/admin");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}



    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }
    




