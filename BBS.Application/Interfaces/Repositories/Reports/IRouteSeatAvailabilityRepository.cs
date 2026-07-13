using BBS.Application.Models.Reports;

namespace BBS.Application.Interfaces.Repositories.Reports
{
    /// <summary>
    /// Repository for reporting and dashboard related operations.
    /// </summary>
    public interface IRouteSeatAvailabilityRepository
    {
        /// <summary>
        /// Gets seat availability details for all scheduled buses
        /// belonging to a specific route.
        /// </summary>
        /// <param name="routeId">Route Id.</param>
        /// <returns>Collection of seat availability information.</returns>
        Task<IReadOnlyList<RouteSeatAvailabilityReportModel>> GetRouteSeatAvailabilityAsync(int routeId);
    }
}
