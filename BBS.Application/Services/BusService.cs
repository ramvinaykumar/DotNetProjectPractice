using BBS.Application.Common;
using BBS.Application.DTOs.Bus;
using BBS.Application.Interfaces.Repositories;
using BBS.Application.Interfaces.Services;
using BBS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace BBS.Application.Services
{
    public class BusService : IBusService
    {
        private readonly IBusRepository _repository;

        private readonly ILogger<BusService> _logger;

        public BusService(IBusRepository repository, ILogger<BusService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<int> CreateAsync(CreateBusRequest request)
        {
            var bus = new Bus
            {
                BusNumber = request.BusNumber,
                BusName = request.BusName,
                TotalSeats = request.TotalSeats
            };

            return await _repository.CreateAsync(bus);
        }

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

        public async Task<BusResponse?> GetByIdAsync(int id)
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

        public async Task UpdateAsync(int id, UpdateBusRequest request)
        {
            var bus = await _repository.GetByIdAsync(id);

            if (bus == null)
                throw new BusinessException("Bus not found");

            bus.BusName = request.BusName;
            bus.BusNumber = request.BusNumber;
            bus.TotalSeats = request.TotalSeats;

            await _repository.UpdateAsync(bus);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
