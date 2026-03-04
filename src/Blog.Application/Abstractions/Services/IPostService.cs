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
    Task<PostDto> FindPostById(Guid Id);
    Task<PostDto> AddAsync(CreatePostDto createPostDto);
    Task<List<PostDto>> ListAllPostAsync();
    Task<PostDto> UpdatePostAsync(Guid Id, UpdatePostDto post);
    Task<bool> DeletePostAsync(Guid Id);
}
