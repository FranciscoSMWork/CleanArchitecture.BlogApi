namespace Blog.Domain.Exceptions;
public class ExceedCaractersNumberException : DomainException
{
    public ExceedCaractersNumberException(string message) : base($"Number of caracthers of {message} exceed the limit")
    {
    }
}
