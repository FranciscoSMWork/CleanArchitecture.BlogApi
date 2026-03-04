using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Comments;
using Blog.Application.DTOs.Posts;
using Blog.Application.DTOs.Users;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces.Repositories;

namespace Blog.Application.Services;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(
        ICommentRepository commentRepository, 
        IUserRepository userRepository, 
        IPostRepository postRepository, 
        IUnitOfWork unitOfWork
        )
    {
        this._commentRepository = commentRepository;
        this._postRepository = postRepository;
        this._userRepository = userRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<CommentDto> FindCommentById(Guid Id)
    {
        Comment comment = await _commentRepository.GetByIdAsync(Id);
        if (comment == null)
            throw new NotFoundException("Comment");

        UserDto authorPostDto = new UserDto
        {
            Id = comment.Post.Author.Id,
            Name = comment.Post.Author.Name,
            Email = comment.Post.Author.Email.Address,
            Bio = comment.Post.Author.Bio
        };

        PostDto postDto = new PostDto
        {
            Id = comment.Post.Id,
            Title = comment.Post.Title,
            Content = comment.Post.Content,
            AuthorId = comment.Post.AuthorId,
            Author = authorPostDto
        };

        UserDto authorCommentDto = new UserDto
        {
            Id = comment.Author.Id,
            Name = comment.Author.Name,
            Email = comment.Author.Email.Address,
            Bio = comment.Author.Bio
        };

        CommentDto commentDto = new CommentDto
        {
            Id = comment.Id,
            PostId = comment.Post.Id,
            Post = postDto,
            AuthorId = comment.Author.Id,
            Author = authorCommentDto,
            Content = comment.Content
        };

        return commentDto;
    }

    public async Task<CommentDto> AddCommentAsync(CommentCreateDto createCommentDto)
    {
        var authorId = createCommentDto.AuthorId;
        User author = await _userRepository.GetByIdAsync(authorId);
        if (author == null)
            throw new NotFoundException("Author");

        var postId = createCommentDto.PostId;
        Post post = await _postRepository.GetByIdAsync(postId);
        if (post == null)
            throw new NotFoundException("Post");

        Comment newComment = new Comment(
            post,
            author, 
            createCommentDto.Content
        );

        Comment createdComment = await _commentRepository.AddAsync(newComment);

        UserDto authorPostDto = new UserDto
        {
            Id = createdComment.Post.Author.Id,
            Name = createdComment.Post.Author.Name,
            Email = createdComment.Post.Author.Email.Address,
            Bio = createdComment.Post.Author.Bio
        };

        PostDto postDto = new PostDto
        {
            Id = createdComment.Post.Id,
            Title = createdComment.Post.Title,
            Content = createdComment.Post.Content,
            AuthorId = createdComment.Post.AuthorId,
            Author = authorPostDto
        };

        UserDto authorCommentDto = new UserDto
        {
            Id = createdComment.Author.Id,
            Name = createdComment.Author.Name,
            Email = createdComment.Author.Email.Address,
            Bio = createdComment.Author.Bio
        };

        CommentDto commentDto = new CommentDto
        {
            Id = createdComment.Id,
            PostId = createdComment.Post.Id,
            Post = postDto,
            AuthorId = createdComment.Author.Id,
            Author = authorCommentDto,
            Content = createdComment.Content
        };

        return commentDto;
    }

    public async Task<List<CommentDto>> ListCommentAsync()
    {
        List<Comment> listComment = await _commentRepository.GetAllAsync();

        List<CommentDto> listCommentDto = listComment
        .Select(comment =>
        {
            var authorPostDto = new UserDto
            {
                Id = comment.Post.Author.Id,
                Name = comment.Post.Author.Name,
                Email = comment.Post.Author.Email.Address,
                Bio = comment.Post.Author.Bio
            };

            var postDto = new PostDto
            {
                Id = comment.Post.Id,
                Title = comment.Post.Title,
                Content = comment.Post.Content,
                AuthorId = comment.Post.AuthorId,
                Author = authorPostDto
            };

            var authorCommentDto = new UserDto
            {
                Id = comment.Author.Id,
                Name = comment.Author.Name,
                Email = comment.Author.Email.Address,
                Bio = comment.Author.Bio
            };

            return new CommentDto
            {
                Id = comment.Id,
                PostId = comment.Post.Id,
                Post = postDto,
                AuthorId = comment.Author.Id,
                Author = authorCommentDto,
                Content = comment.Content
            };
        })
        .ToList();

        return listCommentDto;
    }

    public async Task<bool> DeleteCommentAsync(Guid Id)
    {
        Comment comment = await _commentRepository.GetByIdAsync(Id);
        if (comment == null)
            throw new NotFoundException("Comment");

        return await _commentRepository.DeleteAsync(Id);
    }
}
