using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using MyFirstWebApi.DTOs;

namespace MyFirstWebApi.Services;


public class OrderService : IOrderService
{
    private readonly AppDbContext context;

    public OrderService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Order?> CreateOrder(CreateOrderDto orderDto)
    {
        var user = await context.Users.FindAsync(orderDto.UserId);

        if (user == null)
        {
            return null;
        }

        var order = new Order
        {
            OrderDate = orderDto.OrderDate,
            Amount = orderDto.Amount,
            UserId = orderDto.UserId
        };

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return order;

    }
}