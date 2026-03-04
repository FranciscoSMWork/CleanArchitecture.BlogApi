
namespace Blog.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base($"{message} not found.")
    {
    }
}
