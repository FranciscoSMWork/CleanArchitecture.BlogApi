using Blog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blog.Infrastructure.Infrastructure.Persistence.Converters;

public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() : base
        (
        email => email.Address,
        value => new Email(value)
        )
    {
    }
}
