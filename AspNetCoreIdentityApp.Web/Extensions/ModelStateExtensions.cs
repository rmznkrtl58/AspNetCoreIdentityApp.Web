using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorListExt(this ModelStateDictionary modelState,List<string>errors)
        {
            foreach (var x in errors)
            {
                modelState.AddModelError(string.Empty, x);
            }
        }
        public static void AddModelErrorListExt(this ModelStateDictionary modelState, IEnumerable<IdentityError>errors)
        {
            errors.ToList().ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x.Description);
            });
        }
    }
}
