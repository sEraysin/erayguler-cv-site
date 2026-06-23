using MvcCv.Models.Entity;

namespace MvcCv.Models;

public sealed class AdminDashboardViewModel
{
    public SiteProfile Profile { get; set; } = new();

    public List<Experience> Experiences { get; set; } = [];

    public List<Education> Educations { get; set; } = [];

    public List<Skill> Skills { get; set; } = [];

    public List<Certificate> Certificates { get; set; } = [];

    public List<Project> Projects { get; set; } = [];

    public List<SocialLink> SocialLinks { get; set; } = [];

    public List<BlogPost> BlogPosts { get; set; } = [];

    public List<Message> Messages { get; set; } = [];

    public BlogPost BlogForm { get; set; } = new();
}
