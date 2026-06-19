using BBS.Application.Commands;
using BBS.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// Handles passenger operations including creation, retrieval, updating, and deletion via API endpoints.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : BaseController
    {
        private readonly IMediator _mediator;

        public PassengerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new passenger using the provided command.
        /// </summary>
        /// <param name="command">The command containing passenger details.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePassengerCommand command)
        {
            var result = await _mediator.Send(command);

            return Success(result, "Passenger created successfully.");
        }

        /// <summary>
        /// Retrieves the details of a passenger by unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger.</param>
        /// <returns>An IActionResult containing the passenger details if found; otherwise, an error response.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(
                    new GetPassengerByIdQuery
                    {
                        PassengerId = id
                    });

            return Success(result, "Passenger fetched successfully.");
        }

        /// <summary>
        /// Retrieves all passengers asynchronously.
        /// </summary>
        /// <returns>An IActionResult containing the list of passengers and a success message.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPassengersQuery());

            return Success(result, "Passengers fetched successfully.");
        }

        /// <summary>
        /// Updates the details of an existing passenger identified by the provided ID using the data from the command.
        /// </summary>
        /// <param name="id">int id</param>
        /// <param name="command">The command containing passenger details.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePassengerCommand command)
        {
            command.PassengerId = id;

            var result = await _mediator.Send(command);

            return Success(result, "Passenger updated successfully.");
        }

        /// <summary>
        /// Deletes the passenger with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger to delete.</param>
        /// <returns>An IActionResult indicating the outcome of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(
                new DeletePassengerCommand
                {
                    PassengerId = id
                });

            return Success("Passenger deleted successfully.");
        }
    }
}
