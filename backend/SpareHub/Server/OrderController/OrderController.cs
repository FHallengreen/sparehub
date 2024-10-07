using Domain;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Server.OrderController;

[ApiController]
[Route("/api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    
}
