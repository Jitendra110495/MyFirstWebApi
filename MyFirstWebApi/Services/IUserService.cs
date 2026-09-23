/* using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public interface IUserService
{
    List<User> GetUsers();
} */
using MyFirstWebApi.DTOs;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public interface IUserService
{
    //List<User> GetUsers();
    Task<List<User>> GetUsers();

    Task<User> CreateUser(CreateUserDto userDto);
    Task<User?> GetUserById(int id);
    Task<object> GetUsersPage(string? search, int page, int pageSize);
    Task<User?> UpdateUser(int id, UpdateUserDto userDto);
    Task<User?> PatchUser(int id, PatchUserDto userDto);
    Task<bool> DeleteUser(int id);

    /* User? GetUserById(int id);
    User? UpdateUser(int id, UpdateUserDto userDto);
    User? PatchUser(int id, PatchUserDto userDto);
    bool DeleteUser(int id); */
}