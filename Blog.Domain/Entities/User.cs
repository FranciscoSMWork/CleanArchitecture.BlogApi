using Blog.Domain.Exceptions;
using Blog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities;

public class User
{
    private int maxBioLength = 1000;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public Email Email { get; set; }
    public string Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public User(string _name, Email _email, string _bio)
    {
        Name = _name;
        Email = _email;
        Bio = _bio;
    }

    public void UpdateBio(string newBio)
    {
        if (newBio.Length > maxBioLength)
            throw new ExceedCaractersNumberException("Bio");

        Bio = newBio ?? "";
    }
}
