using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApi.DTOs;

public class CreateOrderDto
{
    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int UserId { get; set; }
}