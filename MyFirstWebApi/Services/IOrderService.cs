using MyFirstWebApi.DTOs;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public interface IOrderService
{
    Task<Order> CreateOrder(CreateOrderDto orderDto);
}