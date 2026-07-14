using BBS.Application.DTOs.Journey;
using BBS.Application.Models.Journey;

namespace BBS.Application.Interfaces.Repositories.Journey
{
    /// <summary>
    /// Repository responsible for passenger journey search.
    /// </summary>
    public interface IJourneyRepository
    {
        /// <summary>
        /// Searches available journeys.
        /// </summary>
        Task<IReadOnlyList<SearchJourneyModel>>  SearchJourneysAsync(   SearchJourneyRequest request);
    }
}
