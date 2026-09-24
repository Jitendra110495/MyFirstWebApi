using MyFirstWebApi.Services;
using MyFirstWebApi.Models;
using MyFirstWebApi.DTOs;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Middleware;
using MyFirstWebApi.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddTransient<IWelcomeService, WelcomeService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<ApplicationSettings>(
    builder.Configuration.GetSection("ApplicationSettings"));


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });


var app = builder.Build();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.UseExceptionHandler("/error");


    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "MyFirstWebApi v1");
    });

app.Run();
public partial class Program { }
internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider)
    : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var authenticationSchemes =
            await authenticationSchemeProvider.GetAllSchemesAsync();

        if (authenticationSchemes.Any(
            authScheme => authScheme.Name == "Bearer"))
        {
            var securitySchemes =
                new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        BearerFormat = "Json Web Token"
                    }
                };


            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = securitySchemes;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security ??= [];

                    operation.Value.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
            }
        }
    }
}

/*Middleware 1
app.Use(async (context, next) =>
{
    Console.WriteLine(
        $"Request: {context.Request.Method} {context.Request.Path}");

    await next();

    Console.WriteLine(
        $"Response Status Code: {context.Response.StatusCode}");
});

/* Middleware 2
app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 2 - Before next");

    await next();

    Console.WriteLine("Middleware 2 - After next");
});
*/
/*
app.Run(async context =>
{
    Console.WriteLine("Middleware 2 is stopping the request.");

    context.Response.StatusCode = 403;

    await context.Response.WriteAsync("You are not allowed.");

    // app.Run() is terminal — it never calls next()
});


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    Console.WriteLine("Endpoint is executing");
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    return forecast;
})
.WithName("GetWeatherForecast");

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware: Request received");

    await next();

    Console.WriteLine("Middleware: Response is going back");
});


app.MapGet("/welcome", (IWelcomeService welcomeService) =>
{
    return welcomeService.GetWelcomeMessage();
});
*/
/* 

var users = new List<User>
{
    new User
    {
        Id = 1,
        Name = "Jitendra",
        Email = "jitendra@example.com"
    },
    new User
    {
        Id = 2,
        Name = "Rahul",
        Email = "rahul@example.com"
    },
    new User
    {
        Id = 3,
        Name = "Amit",
        Email = "amit@example.com"
    }
};

app.MapGet("/users", () =>
{
    return users;
});
 */
// app.MapPost("/users", (User user) =>
// {
//     users.Add(user);

//     return user;
// });

/* app.MapPost("/users", (User user) =>
{
    users.Add(user);

    return Results.Created($"/users/{user.Id}", user);
}); */

/* app.MapPost("/users", (CreateUserDto userDto) =>
{
    var user = new User
    {
        Id = users.Count + 1,
        Name = userDto.Name,
        Email = userDto.Email
    };

    users.Add(user);

    return Results.Created($"/users/{user.Id}", user);
}); */

/* 
app.MapPost("/users", (CreateUserDto userDto) =>
{
    if (string.IsNullOrWhiteSpace(userDto.Name))
    {
        return Results.BadRequest("Name is required.");
    }

    if (string.IsNullOrWhiteSpace(userDto.Email))
    {
        return Results.BadRequest("Email is required.");
    }

    var user = new User
    {
        Id = users.Count + 1,
        Name = userDto.Name,
        Email = userDto.Email
    };

    users.Add(user);

    return Results.Created($"/users/{user.Id}", user);
});
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var existingUser = users.FirstOrDefault(user => user.Id == id);

    if (existingUser == null)
    {
        return Results.NotFound();
    }

    existingUser.Name = updatedUser.Name;
    existingUser.Email = updatedUser.Email;

    return Results.Ok(existingUser);
});
app.MapPatch("/users/{id}", (int id, User updatedUser) =>
{
    var existingUser = users.FirstOrDefault(user => user.Id == id);

    if (existingUser == null)
    {
        return Results.NotFound();
    }

    if (!string.IsNullOrEmpty(updatedUser.Name))
    {
        existingUser.Name = updatedUser.Name;
    }

    if (!string.IsNullOrEmpty(updatedUser.Email))
    {
        existingUser.Email = updatedUser.Email;
    }

    return Results.Ok(existingUser);
});

app.MapDelete("/users/{id}", (int id) =>
{
    var existingUser = users.FirstOrDefault(user => user.Id == id);

    if (existingUser == null)
    {
        return Results.NotFound();
    }

    users.Remove(existingUser);

    return Results.NoContent();
});
app.Map("/error", () =>
{
    return Results.Problem(
        statusCode: 500,
        title: "Something went wrong."
    );
});
 */
//app.Run();
//Testing Git
//Testing Git 2

