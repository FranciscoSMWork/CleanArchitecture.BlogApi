namespace Blog.Domain.Exceptions;
public class AlreadyExistsException : DomainException
{
    public AlreadyExistsException(string message) : base($"{message} already exists.")
    {
    }
}
