using System.ComponentModel.DataAnnotations;

namespace BBS.Application.DTOs.Journey
{
    /// <summary>
    /// Represents a passenger journey search request.
    /// </summary>
    public class SearchJourneyRequest
    {
        /// <summary>
        /// Journey source city.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Source { get; set; } = string.Empty;

        /// <summary>
        /// Journey destination city.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Travel date.
        /// </summary>
        [Required]
        public DateOnly TravelDate { get; set; }
    }
}
