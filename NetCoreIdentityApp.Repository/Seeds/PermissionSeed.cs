using AspNetCoreIdentityApp.Core.Permissions;
using AspNetCoreIdentityApp.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Seeds
{
    public class PermissionSeed
    {
        public static async Task Seed(RoleManager<AppRole>_roleManager)
        {
            var hasBasicRole = await _roleManager.RoleExistsAsync("BasicRole");
            var hasAdvancedRole = await _roleManager.RoleExistsAsync("AdvancedRole");
            var hasAdminRole = await _roleManager.RoleExistsAsync("AdminRole");

            if (!hasBasicRole)//BasicRole adında rol yoksa
            {
                var createRole = new AppRole()
                {
                    Name = "BasicRole"
                };
                await _roleManager.CreateAsync(createRole);
                var findRole = await _roleManager.FindByNameAsync("BasicRole");

                await AddReadPermission(findRole!,_roleManager);
            }

            if (!hasAdvancedRole)//BasicRole adında rol yoksa
            {
                var createRole = new AppRole()
                {
                    Name = "AdvancedRole"
                };
                await _roleManager.CreateAsync(createRole);
                var findRole = await _roleManager.FindByNameAsync("AdvancedRole");

                await AddReadPermission(findRole!, _roleManager);
                await AddUpdateAndCreatePermission(findRole!, _roleManager);
            }

            if (!hasAdminRole)//BasicRole adında rol yoksa
            {
                var createRole = new AppRole()
                {
                    Name = "AdminRole"
                };
                await _roleManager.CreateAsync(createRole);
                var findRole = await _roleManager.FindByNameAsync("AdminRole");

                await AddReadPermission(findRole!, _roleManager);
                await AddUpdateAndCreatePermission(findRole!, _roleManager);
                await AddDeletePermission(findRole!, _roleManager);
            }
        }
        public static async Task AddReadPermission(AppRole appRole,RoleManager<AppRole> roleManager) 
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission"
               , Permissions.Order.Read));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Catalog.Read));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Stock.Read));
        }
        public static async Task AddUpdateAndCreatePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission"
             , Permissions.Order.Create));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Catalog.Create));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Stock.Create));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission"
            , Permissions.Order.Update));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Catalog.Update));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Stock.Update));
        }
        public static async Task AddDeletePermission(AppRole appRole, RoleManager<AppRole> roleManager)
        {
            await roleManager.AddClaimAsync(appRole!, new Claim("Permission"
            , Permissions.Order.Delete));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Catalog.Delete));

            await roleManager.AddClaimAsync(appRole!, new Claim("Permission", Permissions.Stock.Delete));
        }
    }
}
