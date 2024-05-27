using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler:IRequestHandler<LoginUserCommandRequest,LoginUserCommandResponse>
    {
        readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _signInManager;

        public LoginUserCommandHandler(UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> userManager, SignInManager<ETicaretAPI.Domain.Entities.Identity.AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            ETicaretAPI.Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
                throw new NotFoundUserException("Kullanıcı adı ya da Email hatalı...");
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signInResult.Succeeded)//Authentication başarılı.
            {
                //todo aslında burada yetkileri belirlememmiz gerekiyor.
            }
            return new ();
        }
    }
}
