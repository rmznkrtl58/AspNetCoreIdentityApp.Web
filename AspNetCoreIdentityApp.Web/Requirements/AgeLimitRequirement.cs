using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Requirements
{
    public class AgeLimitRequirement:IAuthorizationRequirement
    {   //Yaş Sınırı
        public int AgeLimit{ get; set; }
    }
    public class AgeLimitRequirementHandler : AuthorizationHandler<AgeLimitRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeLimitRequirement requirement)
        {
            //ilgili kullanıcıda birthdate adlı claim Mevcut mu?
            if (!context.User.HasClaim(x => x.Type == "birthDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            Claim birthDateClaim = context.User.FindFirst("birthDate")!;
            var today = DateTime.Now;
            var birthDate = Convert.ToDateTime(birthDateClaim.Value);
            var age=today.Year-birthDate.Year;
            //Doğum Tarihinden bugünün tarihini yaşı kadar çıkartıyorum şart sağlanırsa yaşını bir eksilt
            if (birthDate > today.AddYears(-age)) age--;
            //Yaş sınırım 18 ise gelen yaş değeri bundan küçük ise fail olsun
            if (requirement.AgeLimit > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
