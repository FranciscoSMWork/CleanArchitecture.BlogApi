using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.ValueObjects;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Repositories.Implementations;
using Blog.Infrastructure.Tests.Fixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Tests.Repositories;

public class CommentRepositoryTests
{
    private readonly BlogDbContext _context;
    private readonly ICommentRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentRepositoryTests()
    {
        _context = DbContextFactory.CreateSqlServerInMemory();
        _context.Database.EnsureCreated();

        _repository = new CommentRepository(_context);
        _unitOfWork = new UnitOfWork(_context);
    }

    //Salvar usuário
    [Fact]
    public async Task SaveComment_IfDatasAreCorrect_ShouldReturnComment()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string userName = "Name user test";
        string bio = "Bio test";
        User user = new User(userName, emailCreated, bio);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        string contentComment = "Content Test";
        Comment comment = new Comment(post, user, contentComment);

        //Act
        await _repository.AddAsync(comment);
        await _unitOfWork.CommitAsync();

        //Assert
        Comment? commentReturned = await _context.Comments.FindAsync(comment.Id);
        commentReturned.Author.Name.Should().Be(comment.Author.Name);
        commentReturned.Author.Bio.Should().Be(comment.Author.Bio);
        commentReturned.Author.Email.Address.Should().Be(comment.Author.Email.Address);
    }

    //Buscar usuário por email
    [Fact]
    public async Task FindComment_WhenIdExists_ShouldReturnComment()
    {
        //Arrange
        string email = "test@email.com";
        Email emailCreated = new Email(email);

        string nameUser = "User Test";
        string bioUser = "Bio User";
        User user = new User(nameUser, emailCreated, bioUser);

        string title = "Title Test";
        string content = "Content Test";
        Post post = new Post(title, content, user);

        string contentComment = "Content Test";
        Comment comment = new Comment(post, user, contentComment);

        await _context.Comments.AddAsync(comment);
        await _unitOfWork.CommitAsync();

        //Act
        Comment commentReturned = await _repository.GetByIdAsync(comment.Id);

        //Assert
        commentReturned.Author.Name.Should().Be(nameUser);
        commentReturned.Author.Bio.Should().Be(bioUser);
        commentReturned.Post.Title.Should().Be(title);
        commentReturned.Post.Content.Should().Be(content);
        commentReturned.Content.Should().Be(contentComment);
    }
}
