using AspNetCoreIdentityApp.Service.ServiceInterfaces;
using AspNetCoreIdentityApp.Service.Services;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class TwoFactorExtensions
    {
        public static IServiceCollection AddTwoFactorExt(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<ITwoFactorService, TwoFactorService>();
            return services;
        }
    }
}
