using AspNetCoreIdentityApp.Web.Entities;
using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentityApp.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesViewModels = roles.Select(x => new RoleListViewModel()
            {
                Name = x.Name,
                Id = x.Id
            }).ToList();
            return View(rolesViewModels);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel p)
        {
            var newRole = new AppRole()
            {
                Name = p.Name,
            };
            var createRoleResult = await _roleManager.CreateAsync(newRole);
            if (!createRoleResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(createRoleResult.Errors);
                return View();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRole(string id)
        {
            var findRole = await _roleManager.FindByIdAsync(id);
            if (findRole is null)
            {
                throw new Exception("İlgili Id'ye Ait Kullanıcı Bulunamamıştır!");
            }
            var setViewModel = new UpdateRoleViewModel()
            {
                Id = findRole.Id,
                Name = findRole.Name
            };
            return View(setViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel p)
        {
            var findRole = await _roleManager.FindByIdAsync(p.Id);
            if (findRole is null)
            {
                throw new Exception("İlgili Id'ye Ait Kullanıcı Bulunamamıştır!");
            }
            findRole.Name = p.Name;
            var updateRoleResult = await _roleManager.UpdateAsync(findRole);
            if (!updateRoleResult.Succeeded)
            {
                ModelState.AddModelErrorListExt(updateRoleResult.Errors);
                return View();
            }
            ViewData["SuccessMessage"] = "Role Başarıyla Güncellenmiştir";
            return View();
        }
        public async Task<IActionResult> DeleteRole(string id)
        {
            var findRole = await _roleManager.FindByIdAsync(id);
            if (findRole is null)
            {
                throw new Exception("İlgili Id'ye Ait Rol Bulunamamıştır!");
            }

            var deleteRoleResult = await _roleManager.DeleteAsync(findRole);
            if (!deleteRoleResult.Succeeded)
            {
                throw new Exception(deleteRoleResult.Errors.Select(x => x.Description).First());
            }
            TempData["successMessage"] = "Rol Silinmiştir.";
            return RedirectToAction(nameof(RoleController.Index));
        }
        [HttpGet]
        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            ViewBag.userId= id;
            var findUser = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();
            //İlgili Kullanıcıma Ait rolleri userRoles değişkenime atıyorum.
            var userRoles = await _userManager.GetRolesAsync(findUser);
            var rolViewModelList = new List<AssignRoleToUserViewModel>();
            foreach (var x in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                };
                if (userRoles.Contains(x.Name))
                {
                    //Kullanıcım Ait Rollerin içerisinde rollerdeki isimlerden hangileri varsa true işaretle
                    assignRoleToUserViewModel.Exist = true;
                }
                rolViewModelList.Add(assignRoleToUserViewModel);
            }
            return View(rolViewModelList);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(List<AssignRoleToUserViewModel>pList,string userId)
        {
            var findUser=await _userManager.FindByIdAsync(userId);
            foreach(var x in pList)
            {
                if (x.Exist)//eğerki checkboxum işaretli ise 
                {
                    await _userManager.AddToRoleAsync(findUser!, x.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(findUser!, x.Name);
                }
            }
            return RedirectToAction("UserList","Default");
        }
    }
}
