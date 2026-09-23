/* using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly List<User> users = new List<User>
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
        }
    };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }
} */



using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Services;
using MyFirstWebApi.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.GetUsers();
        return Ok(users);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            message = "Welcome Admin!"
        });
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task <IActionResult> CreateUser(CreateUserDto userDto)
    {
        var user = await userService.CreateUser(userDto);

        return Created($"/api/users/{user.Id}", user);
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await userService.GetUserById(id);

        if (user == null)
        {
        return NotFound();
        }

    return Ok(user);
    }
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task <IActionResult> UpdateUser(int id, UpdateUserDto userDto)
    {
        var user = await userService.UpdateUser(id, userDto);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PatchUser(int id, PatchUserDto userDto)
    {
        var user = await userService.PatchUser(id, userDto);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await userService.DeleteUser(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    } 
/*     [HttpGet("paged")]
    public async Task<IActionResult> GetUsersPage(int page = 1, int pageSize = 10)
    {
        var users = await userService.GetUsersPage(page, pageSize);

        return Ok(users);
    } */
    [HttpGet("paged")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersPage(string? search, int page = 1, int pageSize = 10)
    {
        var result = await userService.GetUsersPage(search, page, pageSize);

        return Ok(result);
    }
/*     [HttpGet]
    public IActionResult GetUsers()
    {
        var users = userService.GetUsers();

        return Ok(users);
    } */

/* 
    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        var user = userService.GetUserById(id);

        if (user == null)
        {
        return NotFound();
    }

    return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserDto userDto)
    {
        var user = userService.CreateUser(userDto);

        return Created($"/api/users/{user.Id}", user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, UpdateUserDto userDto)
    {
        var user = userService.UpdateUser(id, userDto);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchUser(int id, PatchUserDto userDto)
    {
        var user = userService.PatchUser(id, userDto);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var deleted = userService.DeleteUser(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    } */
}