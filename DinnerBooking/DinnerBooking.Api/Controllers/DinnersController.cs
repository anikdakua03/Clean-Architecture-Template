using Microsoft.AspNetCore.Mvc;

namespace DinnerBooking.Api.Controllers;

[Route("[controller]")]
public class DinnersController : ApiController
{
    [HttpGet]
    public IActionResult ListDinners()
    {
        return Ok(new List<string> { "Dinner 1", "Dinner 2" });
    }
}