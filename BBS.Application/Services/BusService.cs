using BBS.Application.Common;
using BBS.Application.DTOs.Bus;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    /// <summary>
    /// Provides operations for creating, retrieving, updating, and deleting bus records.
    /// </summary>
    /// <remarks>Interacts with the bus repository and handles business logic for bus management.</remarks>
    public class BusService : IBusService
    {
        private readonly IBusRepository _repository;
        private readonly ILogger<BusService> _logger;

        /// <summary>
        /// Parameterized constructor for initializing the bus service with the specified repository and logger.
        /// </summary>
        /// <param name="repository">IBusRepository repository</param>
        /// <param name="logger">ILogger<BusService> logger</param>
        public BusService(IBusRepository repository, ILogger<BusService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously creates a new bus record and retrieves the corresponding bus data.
        /// </summary>
        /// <param name="request">The details of the bus to create.</param>
        /// <returns>A task representing the asynchronous operation, containing the created bus data or null if not found.</returns>
        public async Task<BusResponse?> CreateAsync(CreateBusRequest request)
        {
            var bus = new Bus
            {
                BusNumber = request.BusNumber,
                BusName = request.BusName,
                TotalSeats = request.TotalSeats
            };

            var busId = await _repository.CreateAsync(bus);
            return await GetBusDataById(busId) ;
        }

        /// <summary>
        /// Asynchronously retrieves all buses.
        /// </summary>
        /// <returns>A collection of bus response objects.</returns>
        public async Task<IEnumerable<BusResponse>> GetAllAsync()
        {
            var buses = await _repository.GetAllAsync();

            return buses.Select(x =>
                new BusResponse
                {
                    BusId = x.BusId,
                    BusName = x.BusName,
                    BusNumber = x.BusNumber,
                    TotalSeats = x.TotalSeats
                });
        }

        /// <summary>
        /// Asynchronously retrieves a bus response by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the bus.</param>
        /// <returns>A task representing the asynchronous operation, containing the bus response if found; otherwise, null.</returns>
        public async Task<BusResponse?> GetByIdAsync(int id)
        {
            return await GetBusDataById(id);
        }

        /// <summary>
        /// Updates the details of a bus with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the bus to update.</param>
        /// <param name="request">The updated bus information.</param>
        /// <returns>The updated bus data if successful; otherwise, null.</returns>
        /// <exception cref="BusinessException">Thrown when a bus with the specified identifier is not found.</exception>
        public async Task<BusResponse?> UpdateAsync(int id, UpdateBusRequest request)
        {
            var bus = await _repository.GetByIdAsync(id);

            if (bus == null)
                throw new BusinessException("Bus not found");

            bus.BusName = request.BusName;
            bus.BusNumber = request.BusNumber;
            bus.TotalSeats = request.TotalSeats;

            await _repository.UpdateAsync(bus);
            return await GetBusDataById(id);
        }

        /// <summary>
        /// Deletes a bus record with the specified identifier.
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Retrieves bus details for the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the bus.</param>
        /// <returns>A BusResponse containing the bus details, or null if not found.</returns>
        /// <exception cref="BusinessException">Thrown when no bus exists with the specified identifier.</exception>
        private async Task<BusResponse?> GetBusDataById(int id)
        {
            var bus = await _repository.GetByIdAsync(id);

            if (bus == null)
                throw new BusinessException("Bus not found");

            return new BusResponse
            {
                BusId = bus.BusId,
                BusName = bus.BusName,
                BusNumber = bus.BusNumber,
                TotalSeats = bus.TotalSeats
            };
        }
    }
}
