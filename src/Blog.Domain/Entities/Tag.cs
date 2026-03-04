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
    public Tag(string name, string slug)
    {
        SetName(name);
        SetSlug(slug);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValueCannotBeEmptyException("Name");

        this.Name = name;
    }

    private void SetSlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            throw new ValueCannotBeEmptyException("Slug");

        this.Slug = slug;
    }
}
