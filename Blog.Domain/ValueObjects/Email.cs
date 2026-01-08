using Blog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.ValueObjects;
public class Email
{
    public string Address { get; private set; }

    public Email(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ValueCannotBeEmptyException("Email address");

        if (!IsValidEmail(address))
            throw new ValueIsInvalidException("email address");

        Address = address;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public override string ToString()
    {
        return Address;
    }
}
