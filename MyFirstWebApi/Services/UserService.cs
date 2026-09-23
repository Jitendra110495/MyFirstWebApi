using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using MyFirstWebApi.DTOs;
using Microsoft.Extensions.Options;
using MyFirstWebApi.Configuration;
using Microsoft.AspNetCore.Identity;

namespace MyFirstWebApi.Services;


public class UserService : IUserService
{
    private readonly AppDbContext context;
    private readonly ApplicationSettings settings;
    private readonly ILogger<UserService> _logger;

    public UserService(
    AppDbContext context,
    ILogger<UserService> logger)
    {
     this.context = context;
    _logger = logger;
    }

    public async Task<List<User>> GetUsers()
    {
    _logger.LogInformation("Getting all users");
    return await context.Users.ToListAsync();
    }

    public async Task<User> CreateUser(CreateUserDto userDto)
    { 
        var passwordHasher = new PasswordHasher<User>();

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Role = "User"
        };

        user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);
        
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
    public async Task<User?> GetUserById(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<List<User>> GetUsersByName(string name)
    {
        return await context.Users
            .Where(user => user.Name == name)
            .ToListAsync();
    }
    public async Task<List<User>> GetUsersSortedByName()
    {
        return await context.Users
            .OrderBy(user => user.Name)
            .ToListAsync();
    }
    public async Task<User?> FindUserByEmail(string email)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Email == email);
    }
    public async Task<int> GetUserCount()
    {
        return await context.Users.CountAsync();
    }

    public async Task<bool> UserEmailExists(string email)
    {
        return await context.Users
            .AnyAsync(user => user.Email == email);
    }
    public async Task<object> GetUserNames()
    {
        return await context.Users
            .Select(user => new
            {
                user.Id,
                user.Name
            })
        .ToListAsync();
    }
    
    public async Task<List<User>> SearchUsers(string search)
    {
        return await context.Users
            .Where(user => user.Name.Contains(search))
            .ToListAsync();
    }

    
    public async Task<object> GetUsersPage(string? search, int page, int pageSize)
    {
    var query = context.Users.AsQueryable();

    if (!string.IsNullOrEmpty(search))
    {
        query = query.Where(user => user.Name.Contains(search));
    }

    var totalUsers = await query.CountAsync();

    var users = await query
        .OrderBy(user => user.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new
    {
        page,
        pageSize,
        totalUsers,
        users
    };
    }
    
    
    public async Task<User?>UpdateUser(int id, UpdateUserDto userDto)
    {
        var existingUser = await context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;

        await context.SaveChangesAsync();

        return existingUser;
    }

    public async Task<User?> PatchUser(int id, PatchUserDto userDto)
    {
        var existingUser = await context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return null;
        }

        if (userDto.Name != null)
        {
            existingUser.Name = userDto.Name;
        }

        if (userDto.Email != null)
        {
            existingUser.Email = userDto.Email;
        }
        await context.SaveChangesAsync();
        return existingUser;
    }
    public async Task<bool> DeleteUser(int id)
    {
        var existingUser = await context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return false;
        }

        context.Users.Remove(existingUser);
        await context.SaveChangesAsync();
        return true;

    }
    /* private readonly List<User> users = new List<User>
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

    public List<User> GetUsers()
    {
        return users;
    }
 */
    /* public User CreateUser(CreateUserDto userDto)
    {
    var user = new User
    {
        Id = users.Count + 1,
        Name = userDto.Name,
        Email = userDto.Email
    };

    users.Add(user);

    return user;
    }

    public User? GetUserById(int id)
    {
        return users.FirstOrDefault(user => user.Id == id);
    }

    public User? UpdateUser(int id, UpdateUserDto userDto)
    {
        var existingUser = users.FirstOrDefault(user => user.Id == id);

        if (existingUser == null)
        {
            return null;
        }

        existingUser.Name = userDto.Name;
        existingUser.Email = userDto.Email;

        return existingUser;
    }

    public User? PatchUser(int id, PatchUserDto userDto)
    {
        var existingUser = users.FirstOrDefault(user => user.Id == id);

        if (existingUser == null)
        {
            return null;
        }

        if (userDto.Name != null)
        {
            existingUser.Name = userDto.Name;
        }

        if (userDto.Email != null)
        {
            existingUser.Email = userDto.Email;
        }

        return existingUser;
    }
    public bool DeleteUser(int id)
    {
        var existingUser = users.FirstOrDefault(user => user.Id == id);

        if (existingUser == null)
        {
            return false;
        }

        users.Remove(existingUser);

        return true;
    } */
}













