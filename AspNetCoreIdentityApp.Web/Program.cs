using AspNetCoreIdentityApp.Web.Entities;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Seeds;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Identity Ve Claim Ext. Yap�land�rmas�
builder.Services.AddIdentityExt(builder.Configuration).AddClaimExt(builder.Configuration);

var app = builder.Build();

//neden scope=>��nk� memoryde olu�turulan nesneler bir defa i�leme al�ns�n sonra memoryden d��s�n
//Uygulama �al��t���nda bir kere SeedDatam� olu�turcak dahada olu�turmayacak
using (var scope=app.Services.CreateScope())
{

    var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await PermissionSeed.Seed(roleManager);

}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Kimlik Do�rulama s�ra �nemlidir ilk authentication sonra authorizatio gelir.
app.UseAuthentication();
//Kimlik Yetkilendirme
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
