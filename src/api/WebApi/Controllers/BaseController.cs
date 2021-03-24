using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utils;

namespace WebApi.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        protected new IActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected IActionResult FromResult(Result result)
        {
            return result.IsSuccess 
                ? Ok() 
                : Error(result.Error);
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            return result.IsSuccess
                ? Ok(result.Value)
                : Error(result.Error);
        }
    }
}
