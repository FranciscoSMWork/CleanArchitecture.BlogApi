using Blog.Application.Abstractions.Services;
using Blog.Application.DTOs.Posts;
using Blog.Application.DTOs.Users;
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
    public async Task<PostDto> FindPostById(Guid Id)
    {
        Post post = await _postRepository.GetByIdAsync(Id);
        if (post == null)
            throw new NotFoundException("Post");

        UserDto userDto = new UserDto
        {
            Id = post.Author.Id,
            Name = post.Author.Name,
            Email = post.Author.Email.Address,
            Bio = post.Author.Bio
        };

        PostDto postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.Author.Id,
            Author = userDto
        };
        return postDto;
    }
    public async Task<PostDto> AddAsync(CreatePostDto createPostDto)
    {
        User author = await _userRepository.GetByIdAsync(createPostDto.AuthorId);

        if (author == null)
            throw new NotFoundException("Author");

        Post post = new Post(createPostDto.Title, createPostDto.Content, author);
        Post createdPost = await _postRepository.AddAsync(post);

        UserDto userDto = new UserDto
        {
            Id = createdPost.Author.Id,
            Name = createdPost.Author.Name,
            Email = createdPost.Author.Email.Address,
            Bio = createdPost.Author.Bio
        };

        PostDto postResultDto = new PostDto
        {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Content = createdPost.Content,
            AuthorId = createdPost.Author.Id,
            Author = userDto
        };

        return postResultDto;
    }
    public async Task<List<PostDto>> ListAllPostAsync()
    {
        List<PostDto> listPost = new List<PostDto>();
        List<Post> posts = await _postRepository.ListAllAsync();
        
        foreach(var post in posts)
        {
            UserDto userDto = new UserDto
            {
                Id = post.Author.Id,
                Name = post.Author.Name,
                Email = post.Author.Email.Address,
                Bio = post.Author.Bio
            };

            listPost.Add(new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                AuthorId = post.Author.Id,
                Author = userDto
            });
        }

        return listPost;
    }
    public async Task<PostDto> UpdatePostAsync(Guid Id, UpdatePostDto updatePostDto)
    {
        var post = await _postRepository.GetByIdAsync(Id);

        if (post == null || post.DeletedAt != null)
            throw new NotFoundException("Post");

        if (updatePostDto.Content != null)
            post.UpdateContent(updatePostDto.Content);
        
        if (updatePostDto.Title != null)
            post.UpdateTitle(updatePostDto.Title);
        
        await _unitOfWork.CommitAsync();

        UserDto userDto = new UserDto
        {
            Id = post.Author.Id,
            Name = post.Author.Name,
            Email = post.Author.Email.Address,
            Bio = post.Author.Bio
        };

        PostDto postDto = new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            AuthorId = post.Author.Id,
            Author = userDto
        };

        return postDto;
    }
    public async Task<bool> DeletePostAsync(Guid Id)
    {
        return await _postRepository.DeleteAsync(Id);
    }
}
