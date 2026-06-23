# Eray Guler CV Site

ASP.NET Core MVC, Identity, EF Core Code-First ve MySQL ile gelistirilmis dinamik CV, blog ve admin paneli uygulamasi.

## Gereksinimler

- .NET 8 SDK
- MySQL Server 8+
- Git

## Lokal Kurulum

1. Repoyu klonlayin.

```bash
git clone https://github.com/sEraysin/erayguler-cv-site.git
cd erayguler-cv-site
```

2. MySQL'de `erayguler_cv` veritabanini olusturun.

3. Veritabanini olusturun.

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

MySQL provider kaynakli migration kilit hatasi alirsaniz `Migrations/InitialMySqlCodeFirst.full.sql` dosyasini MySQL'de calistirabilirsiniz.

4. Uygulamayi baslatin.

```bash
dotnet run
```

5. Admin paneline girin.

```text
Adres: /admin/giris
Kullanici adi: sEraysin
Sifre: DemoAdmin123!
```

Bu demo admin bilgisi uygulama ilk kez calisirken seed edilir ve sadece lokal gelistirme icindir. Canli ortamda ayni sifreyi kullanmayin. Daha once farkli sifreyle olusturulmus bir lokal veritabaniniz varsa demo sifrenin gelmesi icin veritabanini silip yeniden migration calistirin veya kendi admin sifrenizi kullanin.

## Baglanti Ayari

Varsayilan lokal baglanti `appsettings.Development.json` icindedir:

```bash
server=localhost;port=3306;database=erayguler_cv;user=root;password=;
```

Farkli bir MySQL kullanicisi veya sifresi varsa `appsettings.Development.json` dosyasindaki `DefaultConnection` degerini kendi bilgisayariniza gore guncelleyin.

## Canli Ortam Notlari

- Production sifreleri repoya yazilmamalidir.
- Hosting panelinde `ConnectionStrings__DefaultConnection`, `AdminUser__UserName`, `AdminUser__Email` ve `AdminUser__Password` ortam degiskenleri tanimlanmalidir.
- Ilk admin kullanicisi sadece guvenli `AdminUser:Password` degeri verilirse seed edilir.
- Canliya cikmadan once admin sifresini mutlaka guclu ve benzersiz yapin.
