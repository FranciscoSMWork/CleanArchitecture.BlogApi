using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Comments;
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

    public async Task<Comment> FindCommentById(Guid Id)
    {
       return await _commentRepository.GetByIdAsync(Id);
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

        CommentDto commentResultDto = new CommentDto
        {
            Id = createdComment.Id,
            PostId = createdComment.Post.Id,
            Post = createdComment.Post,
            AuthorId = createdComment.Author.Id,
            Author = createdComment.Author,
            Content = createdComment.Content
        };

        return commentResultDto;
    }

    public async Task<List<CommentDto>> ListCommentAsync()
    {
        List<Comment> listComment = await _commentRepository.GetAllAsync();

        List<CommentDto> listCommentDto = listComment
            .Select(comment => new CommentDto
            {
                Id = comment.Id,
                PostId = comment.Post.Id,
                Post = comment.Post,
                AuthorId = comment.Author.Id,
                Author = comment.Author,
                Content = comment.Content
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
