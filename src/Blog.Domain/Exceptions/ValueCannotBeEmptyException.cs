namespace Blog.Domain.Exceptions;
public class ValueCannotBeEmptyException : DomainException
{
    public ValueCannotBeEmptyException(string message) : base($"{message} cannot be empty.")
    {
    }
}
