namespace MyFirstWebApi.Models;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    public List<Order> Orders { get; set; }
    public string PasswordHash { get; set; }
    public string? Role { get; set; }
}