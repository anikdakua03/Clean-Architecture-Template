using DinnerBooking.Api.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace DinnerBooking.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        // replace the error to HttpContext
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        // create proper problem details and return
        var firstErr = errors[0];
        var statusCode = firstErr.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode :  statusCode, title : firstErr.Description);
    }
}