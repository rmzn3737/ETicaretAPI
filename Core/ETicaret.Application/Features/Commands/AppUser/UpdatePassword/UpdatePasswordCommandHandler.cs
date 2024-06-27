using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Exceptions;
using MediatR;

namespace ETicaret.Application.Features.Commands.AppUser.UpdatePassword
{
    public class UpdatePasswordCommandHandler:IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
    {
        readonly IUserService _userService;

        public UpdatePasswordCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UpdatePasswordCommandResponse> Handle(UpdatePasswordCommandRequest request, CancellationToken cancellationToken)
        {
            if (!request.Password.Equals(request.PasswordConfirm))
            {
                var tip =request.Password.GetType();
                var tip2 = request.PasswordConfirm.GetType();
                throw new PasswordCahngeFailedException("Şifreler eşleşmiyor, lütfen şifreyi doğrulayınız.");
            }
                
           
            await _userService.UpdatePasswordAsync(request.UserId,request.ResetToken,request.Password);
            return new();
        }
    }
}
