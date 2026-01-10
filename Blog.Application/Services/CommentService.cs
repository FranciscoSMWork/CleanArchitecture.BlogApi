using Blog.Application.Interfaces;
using Blog.Domain.Entities;

namespace Blog.Application.Services;
public class CommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(ICommentRepository commentRepository, IUnitOfWork unitOfWork)
    {
        this._commentRepository = commentRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Comment> FindCommentById(int Id)
    {
       return await _commentRepository.GetByIdAsync(Id);
    }

    public Task<bool> AddCommentAsync(Comment comment)
    {
        return _commentRepository.AddAsync(comment);
    }


}
