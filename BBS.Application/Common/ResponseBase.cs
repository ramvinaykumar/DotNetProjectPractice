namespace BBS.Application.Common
{
    /// <summary>
    /// Base class for all response models, providing common properties for reporting and API responses.
    /// </summary>
    public abstract class ResponseBase
    {
        /// <summary>
        /// Total number of records in the report or response. This property is useful for pagination and understanding the size of the dataset.
        /// </summary>
        public int TotalRecords { get; init; }

        /// <summary>
        /// Based on the TotalRecords property, this boolean indicates whether there is any data present in the response. 
        /// It returns true if TotalRecords is greater than zero, otherwise false.
        /// </summary>
        public bool HasData => TotalRecords > 0;

        /// <summary>
        /// Gets the date and time when the object was generated, in Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime GeneratedOnUtc { get; init; }

        /// <summary>
        /// Execution time in milliseconds for generating the report or response. This property can be used for performance monitoring and optimization.
        /// </summary>
        public long ExecutionTimeInMilliseconds { get; init; }

        /// <summary>
        /// Query name or identifier associated with the report or response. This property can be used for logging, debugging, or tracking the source of the data.
        /// </summary>
        public string? QueryName { get; init; }
    }
}
