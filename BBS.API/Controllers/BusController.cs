using BBS.Application.DTOs.Bus;
using BBS.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BBS.API.Controllers
{
    /// <summary>
    /// API controller for managing bus records, supporting retrieval, creation, updating, and deletion operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : BaseController
    {
        private readonly IBusService _service;

        /// <summary>
        /// Initializes a new instance of the BusController class.
        /// </summary>
        /// <param name="service">Service used to manage bus operations.</param>
        public BusController(IBusService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var buses = await _service.GetAllAsync();
            return Success(buses, "Successfully fetched all buses.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bus = await _service.GetByIdAsync(id);
            return Success(bus, "Successfully fetched bus details.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBusRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Success(result, "Successfully created a new bus record.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBusRequest request)
        {
            var result = await _service.UpdateAsync(id, request);

            return Success(result, "Successfully updated the bus record.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Success("Bus data deleted successfully!");
        }
    }
}
