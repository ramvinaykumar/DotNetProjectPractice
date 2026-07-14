using BBS.Application.Common;
using BBS.Application.DTOs.Journey;

namespace BBS.Application.Interfaces.Services
{
    /// <summary>
    /// Journey search service.
    /// </summary>
    public interface IJourneyService
    {
        /// <summary>
        /// Searches available journeys.
        /// </summary>
        Task<QueryResult<SearchJourneyResponse>> SearchJourneysAsync(SearchJourneyRequest request);
    }
}
