using HMS.Core.Dtos.Request.Customer;
using HMS.Core.Dtos.Response;
using HMS.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repo;
        public CustomersController(ICustomerRepository repo) => _repo = repo;

        /// <summary>Get paginated list of customers.</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CustomerQueryRequest query)
        {
            var (customers, total) = await _repo.GetAllAsync(query);
            return Ok(new PagedResponse<Core.Models.Customer>
            {
                Data = customers,
                TotalCount = total,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            });
        }

        /// <summary>Get customer by ID including booking history.</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var (customer, history) = await _repo.GetByIdWithHistoryAsync(id);
            if (customer is null) return NotFound(ApiResponse<object>.Fail("Customer not found."));
            return Ok(ApiResponse<object>.Ok(new { customer, history }));
        }

        /// <summary>Create a new customer.</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest request)
        {
            var id = await _repo.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id },
                ApiResponse<object>.Ok(new { customerId = id }, "Customer created successfully."));
        }

        /// <summary>Update customer details.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateRequest request)
        {
            var rows = await _repo.UpdateAsync(id, request);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Customer not found."));
            return Ok(ApiResponse<object>.Ok(null, "Customer updated successfully."));
        }

        /// <summary>Soft-delete a customer.</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var rows = await _repo.DeleteAsync(id);
            if (rows == 0) return NotFound(ApiResponse<object>.Fail("Customer not found."));
            return Ok(ApiResponse<object>.Ok(null, "Customer deleted successfully."));
        }
    }
}
