using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using FluentAssertions;

namespace Blog.Domain.Tests.Entities;

public class TagTests
{
    [Fact]
    public async Task CreateTag_WhenDatasAreCorrect_ShouldReturnNewTag()
    {
        string Name = "Tag Title";
        string Slug = "Slug Test";

        Tag tag = new Tag(Name, Slug);

        tag.Should().NotBeNull();
        tag.Name.Should().Be(Name);
        tag.Slug.Should().Be(Slug);
    }

    [Fact]
    public async Task CreateTag_WhenNameIsEmpty_ShouldReturnError()
    {
        string Name = "";
        string Slug = "Slug Test";

        Action act = () => new Tag(Name, Slug);

        act.Should()
            .Throw<ValueCannotBeEmptyException>("Name")
            .WithMessage("Name cannot be empty.");
    }

    [Fact]
    public async Task CreateTag_WhenSlugIsEmpty_ShouldReturnError()
    {
        string Name = "Tag Title";
        string Slug = "";

        Action act = () => new Tag(Name, Slug);

        act.Should()
            .Throw<ValueCannotBeEmptyException>("Slug")
            .WithMessage("Slug cannot be empty.");
    }
}
