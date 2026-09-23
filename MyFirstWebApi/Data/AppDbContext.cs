using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    
}