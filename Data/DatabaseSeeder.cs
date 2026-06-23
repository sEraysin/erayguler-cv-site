using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcCv.Models.Context;
using MvcCv.Models.Entity;
using MvcCv.Services;

namespace MvcCv.Data;

public static class DatabaseSeeder
{
    public const string AdminRole = "Admin";

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DbCvContext>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");

        if (configuration.GetValue("Database:ApplyMigrationsOnStartup", false))
        {
            await context.Database.MigrateAsync();
        }

        await SeedIdentityAsync(services, configuration, logger);
        await SeedCvContentAsync(context, logger);
    }

    private static async Task SeedIdentityAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync(AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
        }

        var email = configuration["AdminUser:Email"] ?? "admin@erayguler.com";
        var userName = configuration["AdminUser:UserName"] ?? email;
        var password = configuration["AdminUser:Password"];
        var admin = await userManager.FindByNameAsync(userName) ?? await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                password.StartsWith("SET_", StringComparison.OrdinalIgnoreCase) ||
                password.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning("Admin kullanıcısı oluşturulmadı. Güvenli bir AdminUser:Password değeri user-secrets veya hosting ortam değişkeni ile verilmelidir.");
                return;
            }

            admin = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true,
                FullName = "Eray Güler"
            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Admin kullanıcısı oluşturulamadı: {errors}");
            }
        }
        else
        {
            var changed = false;

            if (!string.Equals(admin.UserName, userName, StringComparison.Ordinal))
            {
                admin.UserName = userName;
                changed = true;
            }

            if (!string.Equals(admin.Email, email, StringComparison.OrdinalIgnoreCase))
            {
                admin.Email = email;
                admin.EmailConfirmed = true;
                changed = true;
            }

            if (changed)
            {
                await userManager.UpdateAsync(admin);
            }
        }

        if (!await userManager.IsInRoleAsync(admin, AdminRole))
        {
            await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }

    private static async Task SeedCvContentAsync(DbCvContext context, ILogger logger)
    {
        if (!await context.SiteProfiles.AnyAsync())
        {
            context.SiteProfiles.Add(new SiteProfile
            {
                FirstName = "Eray",
                LastName = "Güler",
                Title = "Backend Developer | Yönetim Bilişim Sistemleri Öğrencisi",
                Location = "Düzce, Gölyaka",
                Phone = "+90 (544) 365 39 77",
                Email = "gulereray211@gmail.com",
                Language = "İngilizce - Orta Düzey B1",
                ImageUrl = "https://i.hizliresim.com/163rypx.png",
                About = "Düzce Üniversitesi Yönetim Bilişim Sistemleri 4. sınıf öğrencisi olarak akademik altyapımı modern backend teknolojileriyle birleştiriyorum. C#, OOP, LINQ, ASP.NET Core MVC, Web API, EF Core, MSSQL ve MySQL ekosistemlerinde temiz, sürdürülebilir ve ölçeklenebilir çözümler üretmeye odaklanıyorum. Aile işletmesinde edindiğim operasyon, müşteri ilişkileri ve süreç yönetimi deneyimini yazılım tarafında iş analizi, kurumsal mantığı kavrama ve pratik çözüm üretme becerisine dönüştürüyorum."
            });
        }

        if (!await context.Experiences.AnyAsync())
        {
            context.Experiences.AddRange(
                new Experience
                {
                    Title = "Stajyer Yazılım Geliştirici",
                    Company = "Bilsoft Yazılım",
                    DateRange = "Haziran 2026 - Devam Ediyor",
                    Description = "Kurumsal anket süreçlerinin dijitalleştirilmesi için MySQL üzerinde çalışan ASP.NET Core tabanlı web platformu geliştirdim. ASP.NET Core Identity ile Admin ve Kullanıcı rolleri, AccessDenied akışı, EF Core model mapping, seed data, dinamik kullanıcı-personel eşleştirme, filtreleme ekranları ve ClosedXML ile Excel raporlama modülleri üzerinde çalıştım.",
                    DisplayOrder = 1
                },
                new Experience
                {
                    Title = "Operasyon ve Ticari Süreç Yönetimi",
                    Company = "Aile İşletmesi",
                    DateRange = "2018 - Devam Ediyor",
                    Description = "Müşteri ilişkileri, sipariş koordinasyonu, fatura ve belge düzeni, nakit akışı ve kasa takibi süreçlerinde aktif rol aldım. Bu deneyim, yazılım projelerinde süreç analizi yapma, aksaklıkları hızlı tespit etme ve kullanıcı odaklı çözümler geliştirme bakış açımı güçlendirdi.",
                    DisplayOrder = 2
                });
        }

        if (!await context.Educations.AnyAsync())
        {
            context.Educations.Add(new Education
            {
                School = "Düzce Üniversitesi",
                Degree = "Lisans Derecesi",
                Field = "Yönetim Bilişim Sistemleri - 4. Sınıf Öğrencisi",
                Grade = "2.9",
                DateRange = "Eylül 2023 - Devam Ediyor",
                Description = "Yazılım geliştirme, veritabanı yönetim sistemleri, sistem analizi ve tasarımı, veri madenciliği, işletme yönetimi ve dijital dönüşüm odaklı disiplinler arası eğitim.",
                DisplayOrder = 1
            });
        }

        if (!await context.Skills.AnyAsync())
        {
            context.Skills.AddRange(
                Skill("C#", "Core Backend", 92, 1),
                Skill("OOP, LINQ ve SOLID", "Core Backend", 88, 2),
                Skill("ASP.NET Core MVC", "Web & API Frameworks", 90, 3),
                Skill("ASP.NET Core Web API", "Web & API Frameworks", 84, 4),
                Skill("EF Core Code-First / DB-First", "Veritabanı & ORM", 88, 5),
                Skill("MSSQL Server ve MySQL", "Veritabanı & ORM", 86, 6),
                Skill("ASP.NET Core Identity", "Güvenlik & Yetki", 84, 7),
                Skill("HTML5, CSS3, JavaScript", "Ön Yüz Entegrasyonu", 78, 8),
                Skill("Bootstrap ve AdminLTE", "Ön Yüz Entegrasyonu", 82, 9),
                Skill("ViewModels ve Validation", "Ön Yüz Entegrasyonu", 86, 10));
        }

        if (!await context.Certificates.AnyAsync())
        {
            context.Certificates.AddRange(
                Certificate("Asp.Net Core ile Sıfırdan İleri Seviye Web Geliştirme 2026", "Udemy", 1),
                Certificate("C# ile DevExpress'de SQL Tabanlı Ticari Otomasyon Geliştirin", "Udemy", 2),
                Certificate("ASP.NET Core MVC", "BTK Akademi", 3),
                Certificate("Html Css ve JavaScript ile E-Ticaret Sitesi Yapımı", "Udemy", 4),
                Certificate("C# ile 25 Derste 25 Uygulamalı Proje", "Udemy", 5),
                Certificate("Uygulama Geliştirerek C# Öğrenin: A'dan Z'ye Eğitim Seti", "Udemy", 6),
                Certificate("Uygulamalarla Nesne Yönelimli Programlama", "BTK Akademi", 7));
        }

        if (!await context.Projects.AnyAsync())
        {
            context.Projects.AddRange(
                Project("Kişisel CV & Portfolyo Web Sitesi", "ASP.NET Core MVC, Identity, EF Core, MySQL, Bootstrap, AdminLTE", "Teknik yetkinlikleri, projeleri, sertifikaları ve blog içeriklerini dinamik biçimde sunan; Identity korumalı admin paneliyle yönetilebilen production odaklı CV platformu.", "https://github.com/sEraysin", 1),
                Project("Bilsoft Anket Yönetim Platformu", "ASP.NET Core MVC & Razor Pages, EF Core, MySQL, Identity, ClosedXML", "Kurumsal anket verilerinin yetkilendirilmiş rollerle yönetilmesini, filtrelenmesini ve Excel'e aktarılmasını sağlayan web platformu.", "https://github.com/sEraysin", 2),
                Project("AspNetCore-Komple-Web-Gelistirme", "ASP.NET Core MVC, Web API, Razor Pages, EF Core, Identity, Repository Pattern", "Gelişmiş routing, Controller-Action yapısı, ViewModel mimarisi ve API istemci senaryolarının uygulandığı kapsamlı eğitim ve altyapı reposu.", "https://github.com/sEraysin/AspNetCore-Komple-Web-Gelistirme-SadikTuran", 3));
        }

        if (!await context.SocialLinks.AnyAsync())
        {
            context.SocialLinks.AddRange(
                new SocialLink { Name = "GitHub", Url = "https://github.com/sEraysin", IconClass = "fab fa-github", DisplayOrder = 1 },
                new SocialLink { Name = "LinkedIn", Url = "https://linkedin.com/in/eray-guler", IconClass = "fab fa-linkedin-in", DisplayOrder = 2 },
                new SocialLink { Name = "Web", Url = "https://erayguler.com", IconClass = "fas fa-globe", DisplayOrder = 3 });
        }

        if (!await context.BlogPosts.AnyAsync())
        {
            var title = "ASP.NET Core MVC ile Dinamik CV Sitesi Geliştirmek";
            context.BlogPosts.Add(new BlogPost
            {
                Title = title,
                Slug = SlugHelper.Generate(title),
                Summary = "Identity, EF Core, MySQL, AdminLTE ve SEO dostu routing ile modern bir CV platformunun temel bileşenleri.",
                Content = "Bu yazıda ASP.NET Core MVC ile kişisel CV ve portfolyo sitesinin nasıl dinamik hale getirilebileceğini, Identity ile güvenli admin paneli kurulumunu ve EF Core Code-First yaklaşımıyla MySQL veritabanı kullanımını özetliyorum.",
                IsPublished = true,
                PublishedAtUtc = DateTime.UtcNow
            });
        }

        var changes = await context.SaveChangesAsync();
        logger.LogInformation("Seed işlemi tamamlandı. {ChangeCount} kayıt değişikliği işlendi.", changes);
    }

    private static Skill Skill(string name, string category, int level, int order) => new()
    {
        Name = name,
        Category = category,
        Level = level,
        DisplayOrder = order
    };

    private static Certificate Certificate(string name, string issuer, int order) => new()
    {
        Name = name,
        Issuer = issuer,
        DisplayOrder = order
    };

    private static Project Project(string name, string technologies, string description, string url, int order) => new()
    {
        Name = name,
        Technologies = technologies,
        Description = description,
        Url = url,
        DisplayOrder = order
    };
}
