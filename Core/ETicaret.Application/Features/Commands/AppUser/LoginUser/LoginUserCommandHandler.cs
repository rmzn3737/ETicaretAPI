﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Abstractions.Token;
using ETicaret.Application.DTOs;
using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        /*readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> userManager, SignInManager<ETicaretAPI.Domain.Entities.Identity.AppUser> signInManager,
            ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }
*/
        readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password,900);//15 dk lık JWT oluşturuyoruz. 
            return new LoginUserSuccessCommandResponse()
            {
                Token = token
            };
            /*ETicaretAPI.Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
                throw new NotFoundUserException();
            SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signInResult.Succeeded)//Authentication başarılı.
            {
                Token token = _tokenHandler.CreateAccessToken(30);
                //todo aslında burada yetkileri belirlememmiz gerekiyor.
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı adı veya şifre hatalı..."
            //};
            throw new AuthenticationErrorException();*/
        }
    }
}

//***********//
//using ETicaret.Application.Abstractions.Token;
//using ETicaret.Application.DTOs;
//using ETicaret.Application.Exceptions;
//using ETicaret.Application.Features.Commands.AppUser.LoginUser;

////using ETicaretAPI.Application.Abstractions.Token;
////using ETicaretAPI.Application.DTOs;
////using ETicaretAPI.Application.Exceptions;
//using ETicaret.Application.Abstractions.Token;
//using ETicaret.Application.DTOs;
//using ETicaret.Application.Exceptions;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser
//{
//    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
//    {
//        readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
//        readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
//        readonly ITokenHandler _tokenHandler;

//        public LoginUserCommandHandler(
//            UserManager<Domain.Entities.Identity.AppUser> userManager,
//            SignInManager<Domain.Entities.Identity.AppUser> signInManager,
//            ITokenHandler tokenHandler)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _tokenHandler = tokenHandler;
//        }

//        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
//        {
//            Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
//            if (user == null)
//                user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);

//            if (user == null)
//                throw new NotFoundUserException();

//            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
//            if (result.Succeeded) //Authentication başarılı!
//            {
//                Token token = _tokenHandler.CreateAccessToken(5);
//                return new LoginUserSuccessCommandResponse()
//                {
//                    Token = token
//                };
//            }
//            //return new LoginUserErrorCommandResponse()
//            //{
//            //    Message = "Kullanıcı adı veya şifre hatalı..."
//            //};
//            throw new AuthenticationErrorException();
//        }
//    }
//}
