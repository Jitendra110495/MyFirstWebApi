/* namespace MyFirstWebApi.DTOs;

public class CreateUserDto
{
    public string Name { get; set; }

    public string Email { get; set; }
} */

using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApi.DTOs;

/* public class CreateUserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
} */

public class CreateUserDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    public string Password { get; set; }
}