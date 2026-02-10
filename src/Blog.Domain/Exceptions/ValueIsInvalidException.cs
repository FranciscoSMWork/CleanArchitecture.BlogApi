using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Exceptions;

public class ValueIsInvalidException : DomainException
{
    public ValueIsInvalidException(string message) : base($"Invalid {message} format.")
    {
    }
}
