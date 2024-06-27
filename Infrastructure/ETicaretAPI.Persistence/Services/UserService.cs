using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.User;
using ETicaret.Application.Exceptions;
using ETicaret.Application.Features.Commands.AppUser.CreateUser;
using ETicaret.Application.Helpers;
using ETicaretAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService:IUserService
    {
        readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                NameSurname = model.NameSurname,

            }, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)

                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else

                foreach (var error in result.Errors)
                    response.Message += $"{error.Code}-{error.Description}\n";
            //response.Message += $"{error.Code}-{error.Description}<br>";
            return response;
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {
            //AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
            throw new NotFoundUserException();
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordCahngeFailedException();
            }
        }
    }
}
