
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services.Authentications;

namespace ETicaret.Application.Abstractions.Services
{
    public interface IAuthService:IExternalAuthentication, IInternalAuthentication
    {
    }
}
