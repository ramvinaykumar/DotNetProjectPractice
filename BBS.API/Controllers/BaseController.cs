using BBS.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Provides a base class for API controllers with standardized response methods for success and failure scenarios.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Success response with data and a message.
        /// </summary>
        /// <param name="data">T data</param>
        /// <param name="message">string message</param>
        /// <returns>Return response with data and a message</returns>
        protected IActionResult Success<T>(T data, string message)
        {
            return Ok(new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            });
        }

        /// <summary>
        /// Success response with a message only, without any data.
        /// </summary>
        /// <param name="message">string message</param>
        /// <returns>Return response with a message only, without any data.</returns>
        protected IActionResult Success(string message)
        {
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = message
            });
        }

        /// <summary>
        /// Failure response with a message indicating the reason for failure.
        /// </summary>
        /// <param name="message">string message</param>
        /// <returns>Return a message indicating the reason for failure.</returns>
        protected IActionResult Failure(string message)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = message
            });
        }

        /// <summary>
        /// Failure response with a list of validation errors, indicating that the request failed due to validation issues.
        /// </summary>
        /// <param name="errors">list of errors</param>
        /// <returns>Returns a list of validation errors, indicating that the request failed due to validation issues.</returns>
        protected IActionResult Failure(List<string> errors)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation Failed",
                Errors = errors
            });
        }
    }
}
