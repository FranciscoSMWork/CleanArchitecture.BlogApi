using Blog.Domain.Exceptions;

namespace Blog.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    protected Tag() { }
    public Tag(string _name, string _slug)
    {
        if (string.IsNullOrEmpty(_name))
            throw new ValueCannotBeEmptyException("Name");

        if (string.IsNullOrEmpty(_slug))
            throw new ValueCannotBeEmptyException("Slug");

        this.Name = _name;
        this.Slug = _slug;
    }
}
