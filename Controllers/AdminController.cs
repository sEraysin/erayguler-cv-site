using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCv.Data;
using MvcCv.Models;
using MvcCv.Models.Context;
using MvcCv.Models.Entity;
using MvcCv.Services;

namespace MvcCv.Controllers;

[Authorize(Roles = DatabaseSeeder.AdminRole)]
public sealed class AdminController(DbCvContext context, IWebHostEnvironment environment) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = new AdminDashboardViewModel
        {
            Profile = await context.SiteProfiles.AsNoTracking().OrderBy(profile => profile.Id).FirstOrDefaultAsync(cancellationToken) ?? new SiteProfile(),
            Experiences = await context.Experiences.AsNoTracking().OrderBy(experience => experience.DisplayOrder).ToListAsync(cancellationToken),
            Educations = await context.Educations.AsNoTracking().OrderBy(education => education.DisplayOrder).ToListAsync(cancellationToken),
            Skills = await context.Skills.AsNoTracking().OrderBy(skill => skill.DisplayOrder).ToListAsync(cancellationToken),
            Certificates = await context.Certificates.AsNoTracking().OrderBy(certificate => certificate.DisplayOrder).ToListAsync(cancellationToken),
            Projects = await context.Projects.AsNoTracking().OrderBy(project => project.DisplayOrder).ToListAsync(cancellationToken),
            SocialLinks = await context.SocialLinks.AsNoTracking().OrderBy(socialLink => socialLink.DisplayOrder).ToListAsync(cancellationToken),
            BlogPosts = await context.BlogPosts.AsNoTracking().OrderByDescending(blogPost => blogPost.CreatedAtUtc).ToListAsync(cancellationToken),
            Messages = await context.Messages.AsNoTracking().OrderByDescending(message => message.CreatedAtUtc).Take(50).ToListAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpGet("admin/erisim-yok")]
    public IActionResult AccessDenied() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadCv(IFormFile cvFile)
    {
        if (cvFile is null || cvFile.Length == 0)
        {
            return OperationResult(false, "Lütfen bir CV dosyası seçin.", true);
        }

        var extension = Path.GetExtension(cvFile.FileName).ToLowerInvariant();
        string[] allowedExtensions = [".pdf", ".doc", ".docx"];
        if (!allowedExtensions.Contains(extension))
        {
            return OperationResult(false, "CV dosyası PDF, DOC veya DOCX formatında olmalıdır.", true);
        }

        if (cvFile.Length > 5 * 1024 * 1024)
        {
            return OperationResult(false, "CV dosyası en fazla 5 MB olabilir.", true);
        }

        var filesDirectory = Path.Combine(environment.WebRootPath, "files");
        Directory.CreateDirectory(filesDirectory);

        foreach (var existingFile in Directory.GetFiles(filesDirectory, "Eray_Guler_CV.*"))
        {
            System.IO.File.Delete(existingFile);
        }

        var filePath = Path.Combine(filesDirectory, $"Eray_Guler_CV{extension}");
        await using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await cvFile.CopyToAsync(stream);
        }

        return OperationResult(true, "CV dosyası güncellendi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProfile(SiteProfile model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Profil formundaki hataları düzeltin.");

        var profile = await context.SiteProfiles.FirstOrDefaultAsync(item => item.Id == model.Id, cancellationToken);
        if (profile is null)
        {
            context.SiteProfiles.Add(model);
        }
        else
        {
            profile.FirstName = model.FirstName.Trim();
            profile.LastName = model.LastName.Trim();
            profile.Title = model.Title.Trim();
            profile.Location = model.Location.Trim();
            profile.Phone = model.Phone.Trim();
            profile.Email = model.Email.Trim();
            profile.Language = model.Language.Trim();
            profile.ImageUrl = model.ImageUrl.Trim();
            profile.About = model.About.Trim();
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Profil bilgileri güncellendi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveExperience(Experience model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Deneyim formundaki hataları düzeltin.");
        if (model.Id == 0) context.Experiences.Add(model);
        else
        {
            var item = await context.Experiences.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Deneyim bulunamadı.", true);
            item.Title = model.Title.Trim();
            item.Company = model.Company.Trim();
            item.DateRange = model.DateRange.Trim();
            item.Description = model.Description.Trim();
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Deneyim kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteExperience(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Experiences, id, "Deneyim silindi.", "Deneyim bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveEducation(Education model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Eğitim formundaki hataları düzeltin.");
        if (model.Id == 0) context.Educations.Add(model);
        else
        {
            var item = await context.Educations.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Eğitim bulunamadı.", true);
            item.School = model.School.Trim();
            item.Degree = model.Degree.Trim();
            item.Field = model.Field?.Trim() ?? string.Empty;
            item.Grade = model.Grade?.Trim() ?? string.Empty;
            item.DateRange = model.DateRange.Trim();
            item.Description = model.Description?.Trim() ?? string.Empty;
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Eğitim kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteEducation(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Educations, id, "Eğitim silindi.", "Eğitim bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSkill(Skill model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Yetenek formundaki hataları düzeltin.");
        if (model.Id == 0) context.Skills.Add(model);
        else
        {
            var item = await context.Skills.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Yetenek bulunamadı.", true);
            item.Name = model.Name.Trim();
            item.Category = model.Category.Trim();
            item.Level = model.Level;
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Yetenek kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteSkill(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Skills, id, "Yetenek silindi.", "Yetenek bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCertificate(Certificate model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Sertifika formundaki hataları düzeltin.");
        if (model.Id == 0) context.Certificates.Add(model);
        else
        {
            var item = await context.Certificates.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Sertifika bulunamadı.", true);
            item.Name = model.Name.Trim();
            item.Issuer = model.Issuer.Trim();
            item.DateText = model.DateText?.Trim() ?? string.Empty;
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Sertifika kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteCertificate(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Certificates, id, "Sertifika silindi.", "Sertifika bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProject(Project model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Proje formundaki hataları düzeltin.");
        if (model.Id == 0) context.Projects.Add(model);
        else
        {
            var item = await context.Projects.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Proje bulunamadı.", true);
            item.Name = model.Name.Trim();
            item.Technologies = model.Technologies.Trim();
            item.Description = model.Description.Trim();
            item.Url = model.Url?.Trim();
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Proje kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteProject(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Projects, id, "Proje silindi.", "Proje bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveSocialLink(SocialLink model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationError("Sosyal medya formundaki hataları düzeltin.");
        model.IsActive = Request.Form["IsActive"].Any(value => string.Equals(value, "true", StringComparison.OrdinalIgnoreCase));

        if (model.Id == 0) context.SocialLinks.Add(model);
        else
        {
            var item = await context.SocialLinks.FirstOrDefaultAsync(x => x.Id == model.Id, cancellationToken);
            if (item is null) return OperationResult(false, "Sosyal medya kaydı bulunamadı.", true);
            item.Name = model.Name.Trim();
            item.Url = model.Url.Trim();
            item.IconClass = model.IconClass.Trim();
            item.IsActive = model.IsActive;
            item.DisplayOrder = model.DisplayOrder;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Sosyal medya kaydı kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteSocialLink(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.SocialLinks, id, "Sosyal medya kaydı silindi.", "Sosyal medya kaydı bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveBlog(BlogPost model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(BlogPost.Slug));
        ModelState.Remove(nameof(BlogPost.CreatedAtUtc));
        ModelState.Remove(nameof(BlogPost.PublishedAtUtc));
        ModelState.Remove(nameof(BlogPost.UpdatedAtUtc));
        if (!ModelState.IsValid) return ValidationError("Blog formundaki hataları düzeltin.");

        var slug = SlugHelper.Generate(model.Title);
        BlogPost savedBlogPost;
        if (model.Id == 0)
        {
            if (await context.BlogPosts.AnyAsync(blogPost => blogPost.Slug == slug, cancellationToken))
            {
                slug = $"{slug}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            }

            model.Slug = slug;
            model.CreatedAtUtc = DateTime.UtcNow;
            model.PublishedAtUtc = model.IsPublished ? DateTime.UtcNow : null;
            context.BlogPosts.Add(model);
            savedBlogPost = model;
        }
        else
        {
            var blogPost = await context.BlogPosts.FirstOrDefaultAsync(post => post.Id == model.Id, cancellationToken);
            if (blogPost is null) return OperationResult(false, "Blog yazısı bulunamadı.", true);

            var slugOwner = await context.BlogPosts.AnyAsync(post => post.Id != model.Id && post.Slug == slug, cancellationToken);
            blogPost.Title = model.Title.Trim();
            blogPost.Slug = slugOwner ? $"{slug}-{model.Id}" : slug;
            blogPost.Summary = model.Summary.Trim();
            blogPost.Content = model.Content.Trim();
            blogPost.IsPublished = model.IsPublished;
            blogPost.UpdatedAtUtc = DateTime.UtcNow;
            blogPost.PublishedAtUtc ??= model.IsPublished ? DateTime.UtcNow : null;
            savedBlogPost = blogPost;
        }

        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, "Blog yazısı kaydedildi.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteBlog(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.BlogPosts, id, "Blog yazısı silindi.", "Blog yazısı bulunamadı.", cancellationToken);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteMessage(int id, CancellationToken cancellationToken) =>
        DeleteEntity(context.Messages, id, "Mesaj silindi.", "Mesaj bulunamadı.", cancellationToken);

    private async Task<IActionResult> DeleteEntity<TEntity>(DbSet<TEntity> set, int id, string successMessage, string notFoundMessage, CancellationToken cancellationToken)
        where TEntity : class
    {
        var item = await set.FindAsync([id], cancellationToken);
        if (item is null)
        {
            return OperationResult(false, notFoundMessage, true);
        }

        set.Remove(item);
        await context.SaveChangesAsync(cancellationToken);
        return OperationResult(true, successMessage);
    }

    private IActionResult OperationResult(bool success, string message, bool badRequest = false)
    {
        if (IsAjaxRequest())
        {
            return badRequest
                ? BadRequest(new { success, message })
                : Ok(new { success, message });
        }

        TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
        return RedirectToAction(nameof(Index));
    }

    private bool IsAjaxRequest() =>
        string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

    private IActionResult ValidationError(string message)
    {
        var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
        var errorList = errors as string[] ?? errors.ToArray();
        var fullMessage = errorList.Length == 0 ? message : $"{message} {string.Join(" ", errorList)}";

        if (!IsAjaxRequest())
        {
            TempData["ErrorMessage"] = fullMessage;
            return RedirectToAction(nameof(Index));
        }

        return BadRequest(new
        {
            success = false,
            message = fullMessage,
            errors = errorList
        });
    }
}
