using MvcCv.Models.Entity;

namespace MvcCv.Models;

public sealed class CvViewModel
{
    public SiteProfile Profile { get; set; } = new();

    public List<Experience> Experiences { get; set; } = [];

    public List<Education> Educations { get; set; } = [];

    public List<Skill> Skills { get; set; } = [];

    public List<Certificate> Certificates { get; set; } = [];

    public List<Project> Projects { get; set; } = [];

    public List<SocialLink> SocialLinks { get; set; } = [];

    public List<BlogPost> LatestBlogPosts { get; set; } = [];

    public ContactMessageInputModel ContactForm { get; set; } = new();
}
