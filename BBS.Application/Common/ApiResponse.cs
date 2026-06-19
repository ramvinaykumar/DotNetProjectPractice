using System.Text.Json.Serialization;

namespace BBS.Application.Common
{
    /// <summary>
    /// Commnon API response model for standardizing HTTP responses across the application.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Success status of the API response. True if the request was successful; otherwise, false.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the response.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets a collection of error messages.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Gets or sets a collection of validation errors, with property names as keys and arrays of error messages as
        /// values.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }
}
