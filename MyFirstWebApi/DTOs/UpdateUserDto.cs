using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApi.DTOs;

public class UpdateUserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}