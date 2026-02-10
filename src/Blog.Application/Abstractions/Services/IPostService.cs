using Blog.Application.DTOs.Posts;
using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Abstractions.Services;

public interface IPostService
{
    Task<Post> FindPostById(Guid Id);
    Task<PostResultDto> AddAsync(CreatePostDto createPostDto);
    Task<List<Post>> ListAllPostAsync();
    Task<Post> UpdatePostAsync(Guid Id, UpdatePostDto post);
    Task<bool> DeletePostAsync(Guid Id);
}
