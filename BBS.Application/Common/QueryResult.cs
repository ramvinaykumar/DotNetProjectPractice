namespace BBS.Application.Common
{
    /// <summary>
    /// Represents a generic reporting result.
    /// </summary>
    public sealed class QueryResult<T> : ResponseBase
    {
        /// <summary>
        /// Report Items.
        /// </summary>
        public IReadOnlyList<T>? Items { get; init; }
    }
}
