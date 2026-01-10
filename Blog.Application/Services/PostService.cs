using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services;
public class PostService
{

    private readonly IPostRepository _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Post> FindPostById(Guid Id)
    {
        return await _postRepository.GetByIdAsync(Id);
    }
    public async Task<bool> AddAsync(Post post)
    {
        return await _postRepository.AddAsync(post);
    }
    public async Task<List<Post>> ListAllPostAsync()
    {
        return await _postRepository.ListAllAsync();
    }
    public async Task<bool> UpdatePostAsync(Post post)
    {
        return await _postRepository.UpdatePost(post);
    }
    public async Task<bool> DeletePostAsync(Guid Id)
    {
        return await _postRepository.DeleteAsync(Id);
    }
}
