using BBS.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        protected IActionResult Failure<T>(string message)
        {
            return Ok(new ApiResponse<T>
            {
                Success = false,
                Message = message
            });
        }
    }
}
