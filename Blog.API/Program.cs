using Blog.API.Middlewares;
using Blog.Application.Abstractions.Services;
using Blog.Application.Services;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infrastructure.Context;
using Blog.Infrastructure.Persistence;
using Blog.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

/*app.UseMiddleware<ExceptionHandlingMiddleware>();
*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    /*app.UseExceptionHandler("/Error");*/
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();

public partial class Program { }