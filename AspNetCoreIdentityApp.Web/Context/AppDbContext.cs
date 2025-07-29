using AspNetCoreIdentityApp.Web.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Context
{
    public class AppDbContext:IdentityDbContext<AppUser,AppRole,string>//String verme sebebim her bir kullanıcıma random bir guid atayacağımdan dolayı
    {
        //Base=>miras aldığım sınıfın constructorunda belirtmek
        public AppDbContext(DbContextOptions options):base(options) { }
    }
}
