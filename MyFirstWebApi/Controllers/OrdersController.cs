using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Services;
using MyFirstWebApi.DTOs;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }


    [HttpPost]
    public async Task <IActionResult> CreateOrder(CreateOrderDto orderDto)
    {
        var order = await orderService.CreateOrder(orderDto);

        if (order == null)
        {
            return NotFound("User not found.");
        }

        return Created($"/api/Orders/{order.Id}", order);
    }
    } 