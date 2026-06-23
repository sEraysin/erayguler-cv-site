using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCv.Models;
using MvcCv.Models.Context;
using MvcCv.Models.Entity;

namespace MvcCv.Controllers;

[AllowAnonymous]
public sealed class DefaultController(DbCvContext context, IWebHostEnvironment environment) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await BuildCvViewModelAsync(cancellationToken);

        return View(model);
    }

    [HttpGet("hakkimda")]
    public Task<IActionResult> About(CancellationToken cancellationToken) => Section("about", cancellationToken);

    [HttpGet("deneyim")]
    public Task<IActionResult> Experiences(CancellationToken cancellationToken) => Section("experience", cancellationToken);

    [HttpGet("egitim")]
    public Task<IActionResult> Education(CancellationToken cancellationToken) => Section("education", cancellationToken);

    [HttpGet("yetenekler")]
    public Task<IActionResult> Skills(CancellationToken cancellationToken) => Section("skills", cancellationToken);

    [HttpGet("projeler")]
    public Task<IActionResult> Projects(CancellationToken cancellationToken) => Section("projects", cancellationToken);

    [HttpGet("sertifikalar")]
    public Task<IActionResult> Certificates(CancellationToken cancellationToken) => Section("certificates", cancellationToken);

    [HttpGet("iletisim")]
    public Task<IActionResult> Contact(CancellationToken cancellationToken) => Section("contact", cancellationToken);

    [HttpPost("iletisim/gonder")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(ContactMessageInputModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(value => value.Errors).Select(error => error.ErrorMessage);
            var errorList = errors as string[] ?? errors.ToArray();
            var message = errorList.Length == 0
                ? "Lütfen formdaki hataları düzeltin."
                : $"Lütfen formdaki hataları düzeltin. {string.Join(" ", errorList)}";

            if (IsAjaxRequest())
            {
                return BadRequest(new { success = false, message, errors = errorList });
            }

            TempData["ErrorMessage"] = message;
            return RedirectToAction(nameof(Contact));
        }

        context.Messages.Add(new Message
        {
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            Subject = model.Subject.Trim(),
            Body = model.Body.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        });

        await context.SaveChangesAsync(cancellationToken);
        if (IsAjaxRequest())
        {
            return Ok(new { success = true, message = "Mesajınız başarıyla kaydedildi. En kısa sürede dönüş yapacağım." });
        }

        TempData["SuccessMessage"] = "Mesajınız başarıyla kaydedildi. En kısa sürede dönüş yapacağım.";
        return RedirectToAction(nameof(Contact));
    }

    [HttpGet("cv-indir")]
    public IActionResult DownloadCv()
    {
        var filesDirectory = Path.Combine(environment.WebRootPath, "files");
        var cvFile = Directory.Exists(filesDirectory)
            ? Directory.GetFiles(filesDirectory, "Eray_Guler_CV.*")
                .OrderByDescending(System.IO.File.GetLastWriteTimeUtc)
                .FirstOrDefault()
            : null;

        if (cvFile is null)
        {
            return NotFound("CV dosyası bulunamadı.");
        }

        Response.Headers.CacheControl = "no-store, no-cache, must-revalidate, max-age=0";
        Response.Headers.Pragma = "no-cache";
        Response.Headers.Expires = "0";

        var extension = Path.GetExtension(cvFile).ToLowerInvariant();
        var contentType = extension switch
        {
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };

        return PhysicalFile(
            cvFile,
            contentType,
            $"Eray_Guler_CV{extension}");
    }

    private async Task<IActionResult> Section(string sectionId, CancellationToken cancellationToken)
    {
        ViewData["InitialSection"] = sectionId;
        var model = await BuildCvViewModelAsync(cancellationToken);
        return View("Index", model);
    }

    private bool IsAjaxRequest() =>
        string.Equals(Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);

    private async Task<CvViewModel> BuildCvViewModelAsync(CancellationToken cancellationToken)
    {
        return new CvViewModel
        {
            Profile = await context.SiteProfiles.AsNoTracking().OrderBy(profile => profile.Id).FirstOrDefaultAsync(cancellationToken) ?? new SiteProfile(),
            Experiences = await context.Experiences.AsNoTracking().OrderBy(experience => experience.DisplayOrder).ToListAsync(cancellationToken),
            Educations = await context.Educations.AsNoTracking().OrderBy(education => education.DisplayOrder).ToListAsync(cancellationToken),
            Skills = await context.Skills.AsNoTracking().OrderBy(skill => skill.DisplayOrder).ToListAsync(cancellationToken),
            Certificates = await context.Certificates.AsNoTracking().OrderBy(certificate => certificate.DisplayOrder).ToListAsync(cancellationToken),
            Projects = await context.Projects.AsNoTracking().OrderBy(project => project.DisplayOrder).ToListAsync(cancellationToken),
            SocialLinks = await context.SocialLinks.AsNoTracking().Where(link => link.IsActive).OrderBy(link => link.DisplayOrder).ToListAsync(cancellationToken),
            LatestBlogPosts = await context.BlogPosts.AsNoTracking()
                .Where(blogPost => blogPost.IsPublished)
                .OrderByDescending(blogPost => blogPost.PublishedAtUtc ?? blogPost.CreatedAtUtc)
                .Take(3)
                .ToListAsync(cancellationToken)
        };
    }
}
