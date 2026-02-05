using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Posts;
using Blog.Domain.Entities;
using Blog.Domain.Exceptions;
using Blog.Domain.Interfaces.Repositories;

namespace Blog.Application.Services;
public class PostService : IPostService
{

    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IPostRepository postRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Post> FindPostById(Guid Id)
    {
        return await _postRepository.GetByIdAsync(Id);
    }
    public async Task<PostResultDto> AddAsync(CreatePostDto createPostDto)
    {
        User author = await _userRepository.GetByIdAsync(createPostDto.AuthorId);

        if (author == null)
            throw new NotFoundException("Author");

        Post post = new Post(createPostDto.Title, createPostDto.Content, author);
        Post createdPost = await _postRepository.AddAsync(post);


        PostResultDto postResultDto = new PostResultDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Content = createdPost.Content,
            AuthorId = createdPost.Author.Id,
            Author = createdPost.Author
        };

        return postResultDto;
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
