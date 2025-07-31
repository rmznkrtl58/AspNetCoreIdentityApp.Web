using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreIdentityApp.Service.ServiceInterfaces
{
    public interface ITwoFactorService
    {
        public string GenerateQRCodeUri(string email,string unformattedKey);
    }
}
