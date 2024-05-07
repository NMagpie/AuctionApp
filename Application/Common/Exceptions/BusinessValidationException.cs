namespace Application.Common.Exceptions;
public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message) : base(message)
    {

    }

    public BusinessValidationException(string message, Exception e) : base(message, e)
    {

    }
}

