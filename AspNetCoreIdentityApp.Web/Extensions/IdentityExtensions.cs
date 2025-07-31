using AspNetCoreIdentityApp.Repository.Context;
using AspNetCoreIdentityApp.Service.CustomValidations;
using AspNetCoreIdentityApp.Repository.Entities;
using AspNetCoreIdentityApp.Web.Localization;
using AspNetCoreIdentityApp.Core.OptionsModels;
using AspNetCoreIdentityApp.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using AspNetCoreIdentityApp.Service.ServiceInterfaces;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class IdentityExtensions
    {   //Bütün DI Container Yapılandırmamızı buraya yazacağız
        public static IServiceCollection AddIdentityExt(this IServiceCollection services,IConfiguration configuration)
        {   

            //DbContext Yapılandırması
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SqlCon"),settings =>
                {
                    settings.MigrationsAssembly("NetCoreIdentityApp.Repository");
                });
            });

            //Newlemelerden Kurtarmak
            services.AddScoped<IEmailService,EmailService>();
            services.AddScoped<IMemberService,MemberService>();
            //SecurityStamp Yapılandırması
            services.Configure<SecurityStampValidatorOptions>(opt =>
            {   
                //her 30 dkda bir securityStamp değerini kontrol eder.Bir kullanıcı eğerki başka bir tarayıcıdan kullanıcı adını veya önemli bilgilerini güncellerse diğer signIn olan tarayıcıda ise halen giriş yapılıysa 30.dkda cookideki securityStamp değeriyle Db'deki securityStamp değerini karşılaştırır eğerki eşleşmesse login ekranına paslar.
                opt.ValidationInterval = TimeSpan.FromMinutes(30);
            });

            //development.appsettingsJson->Optionsa bağlı EmailSettings gördüğün zaman datayı al
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
            //Identity Yapılandırması
            services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                //Bir kullanıcı adı için olması gereken karakterler
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz123456789_";
                opt.Password.RequiredLength = 6;//şifre en az 6 karakter olmalı!
                opt.Password.RequireNonAlphanumeric = false;//Özel karakter girmesende olur
                opt.Password.RequireLowercase = true;//küçük harf zorunlu
                opt.Password.RequireUppercase = false;//şifrede büyük harf olması zorunlu değil
                opt.Password.RequireDigit = true;//şifrede sayı zorunlu
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);//3 dk kilitli kalma süresi
                opt.Lockout.MaxFailedAccessAttempts = 3;//3 defa üst üste yanlış girilirse kilit mekanizması devreye girsin.
            }).AddPasswordValidator<PasswordValidator>()
                .AddUserValidator<UserValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>()
                 .AddDefaultTokenProviders()//bizim için default bir token üretimi yapacak 
                 ;

            //Cookie Yapılandırması
            services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder();
                cookieBuilder.Name = "IdentityAppCookie";
                opt.Cookie = cookieBuilder;
                //Giriş yapmayan kullanıcılarım üye sayfasına erişmeye çalıştığında giriş yapma sayfasına yönlendircem.
                opt.LoginPath = new PathString("/Login/SignIn");
                opt.LogoutPath = new PathString("/Login/SignOut");
                //İlgili Role Erişimi yoksa İlgili hata sayfasına git
                opt.AccessDeniedPath = new PathString("/ErrorPages/AccessDenied");
                //giriş yapan kullanıcım sahip olduğu cookie ile 60 gün boyunca sayfaya erişim sağlar
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                //eğer false olursa "slidingExpiration"=>cookie 60 günü doldurunca sıfırlanacak biticek süresi ama true=>olduğu için her giriş yaptığı müttetçe 60 gün daha ekler.
                opt.SlidingExpiration = true;
            });

            //herhangi bir dosyaya erişmek için IFilerProvider geçtiğim her classımda dosyaya erişimi kolaylaştırdım.
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            //Şifre yenileme linki için default token üretimi
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                //Identity üzerinde kullanıcıma şifre sıfırlamak için yollacağım linkteki tokenin süresi 1 saat belirledik.
                opt.TokenLifespan = TimeSpan.FromHours(1);
            });

            return services;
        }
    }
}
