namespace CGMD.Models
{
    /// <summary>
    /// Model used for displaying error information.
    /// Includes the request ID for tracing and debugging purposes.
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
