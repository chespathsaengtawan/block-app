namespace BlockApp.Api.Exceptions;
public class InvalidOtpException : Exception
{
    public InvalidOtpException(string message)
        : base(message)
    {
    }
}