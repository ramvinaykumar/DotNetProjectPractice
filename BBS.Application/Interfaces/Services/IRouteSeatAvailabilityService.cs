using BBS.Application.Common;
using BBS.Application.DTOs.Reports;

namespace BBS.Application.Interfaces.Services
{
    /// <summary>
    /// Provides reporting related operations.
    /// </summary>
    public interface IRouteSeatAvailabilityService
    {
        /// <summary>
        /// Returns route-wise seat availability.
        /// </summary>
        /// <param name="routeId">Route Id.</param>
        /// <returns>Seat availability details.</returns>
        Task<QueryResult<RouteSeatAvailabilityResponse>> GetRouteSeatAvailabilityAsync(int routeId);
    }
}
