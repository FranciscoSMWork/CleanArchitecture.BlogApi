using Blog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Fact]
    public async Task CreateEmail_WhenDataIsValid()
    {
        string email = "email@test.com";
        Email emailCreated = new Email(email);

        Assert.NotNull(emailCreated);
    }
}
