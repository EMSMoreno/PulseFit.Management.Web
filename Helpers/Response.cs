namespace PulseFit.Management.Web.Helpers
{
    public class Response
    {
        // Indicates whether the operation was successful or not.
        public bool IsSuccess { get; set; }

        // Contains a descriptive message about the result of the operation.
        public string Message { get; set; }

        // It contains the results of the operation, if any..
        public object Results;
    }
}