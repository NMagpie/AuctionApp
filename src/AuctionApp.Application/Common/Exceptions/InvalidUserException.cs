namespace Application.Common.Exceptions;
public class InvalidUserException : Exception
{
    public InvalidUserException() : base() { }
    public InvalidUserException(string message) : base(message) { }

    public InvalidUserException(string message, Exception e) : base(message, e) { }
}
