using AspNetCoreIdentityApp.Web.ClaimProviders;
using AspNetCoreIdentityApp.Core.Permissions;
using AspNetCoreIdentityApp.Web.Requirements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ClaimExtensions
    {
        public static IServiceCollection AddClaimExt(this IServiceCollection services,IConfiguration configuration)
        {
            //Claim İşlemleri
            services.AddScoped<IClaimsTransformation, UserClaimProvider>();
            services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, AgeLimitRequirementHandler>();
            services.AddAuthorization(opt =>
            {
                //şartname hazırlıyorum.
                opt.AddPolicy("IstanbulPolicy", policy =>
                {
                    //claimlerim arasında key(city)-value(istanbul) olan için bir policy tanımladım.
                    policy.RequireClaim("city", "İstanbul");
                });

                opt.AddPolicy("ExchangeExpireDatePolicy", policy =>
                {
                    policy.AddRequirements(new ExchangeExpireRequirement());
                });
                opt.AddPolicy("AgeLimitPolicy", policy =>
                {
                    policy.AddRequirements(new AgeLimitRequirement()
                    {   //18 yaşından büyük olanlar sayfaya erişebilir.
                        AgeLimit = 18
                    });
                });
                opt.AddPolicy("PermissionReadAndDeletePolicy", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Delete);
                    policy.RequireClaim("permission", Permissions.Order.Read);
                    policy.RequireClaim("permission", Permissions.Stock.Delete);
                });
                opt.AddPolicy("Permissions.Permissions.Order.Delete", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Delete);
                });
                opt.AddPolicy("Permissions.Permissions.Order.Read", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Order.Read);
                });
                opt.AddPolicy("Permissions.Permissions.Stock.Delete", policy =>
                {
                    policy.RequireClaim("permission", Permissions.Stock.Delete);
                });
            });
            return services;
        }
    }
}
