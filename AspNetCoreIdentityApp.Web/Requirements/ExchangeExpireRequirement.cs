using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.Web.Requirements
{
    public class ExchangeExpireRequirement:IAuthorizationRequirement
    {
        
    }
    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            //Kullanıcının üye olduktan sonra 10 gün bekleme claimini Var mı?
            var HasExchangeExpireDateClaim = context.User.HasClaim(x => x.Type == "ExchangeExpireDate");
            //Eğerki Böyle bir claim yoksa
            if (!HasExchangeExpireDateClaim)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            //ilgili kullanıcının claimini bul
            Claim FindExchangeExpireDate = context.User.FindFirst("ExchangeExpireDate")!;
            //Eğerki Claim bu günden küçükse
            if (DateTime.Now > Convert.ToDateTime(FindExchangeExpireDate.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
