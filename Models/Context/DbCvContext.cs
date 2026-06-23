using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcCv.Models.Entity;

namespace MvcCv.Models.Context;

public sealed class DbCvContext : IdentityDbContext<ApplicationUser>
{
    public DbCvContext()
    {
    }

    public DbCvContext(DbContextOptions<DbCvContext> options)
        : base(options)
    {
    }

    public DbSet<SiteProfile> SiteProfiles => Set<SiteProfile>();

    public DbSet<Experience> Experiences => Set<Experience>();

    public DbSet<Education> Educations => Set<Education>();

    public DbSet<Skill> Skills => Set<Skill>();

    public DbSet<Certificate> Certificates => Set<Certificate>();

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BlogPost>()
            .HasIndex(blogPost => blogPost.Slug)
            .IsUnique();

        builder.Entity<SocialLink>()
            .HasIndex(socialLink => socialLink.Name);

        builder.Entity<Message>()
            .HasIndex(message => message.CreatedAtUtc);
    }
}
