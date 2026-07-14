namespace BBS.Application.Common
{
    /// <summary>
    /// Represents errors that occur during business logic execution.
    /// </summary>
    /// <remarks>Use BusinessException to provide error information specific to business rules, including an
    /// optional error code for detailed identification.</remarks>
    public class BusinessException : Exception
    {
        public string ErrorCode { get; }

        public BusinessException(string message) : base(message)
        {
            ErrorCode = string.Empty;
        }

        public BusinessException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
