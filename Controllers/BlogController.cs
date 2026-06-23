using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCv.Models.Context;

namespace MvcCv.Controllers;

[AllowAnonymous]
[Route("blog")]
public sealed class BlogController(DbCvContext context) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var blogPosts = await context.BlogPosts.AsNoTracking()
            .Where(blogPost => blogPost.IsPublished)
            .OrderByDescending(blogPost => blogPost.PublishedAtUtc ?? blogPost.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return View(blogPosts);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken)
    {
        var blogPost = await context.BlogPosts.AsNoTracking()
            .FirstOrDefaultAsync(post => post.Slug == slug && post.IsPublished, cancellationToken);

        return blogPost is null ? NotFound() : View(blogPost);
    }
}
