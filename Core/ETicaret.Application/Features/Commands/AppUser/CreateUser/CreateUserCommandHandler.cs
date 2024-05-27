﻿using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler:IRequestHandler<CreateUserCommandRequest,CreateUserCommandResponse>
    {
        private readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                NameSurname = request.NameSurname,

            }, request.Password);
            
            CreateUserCommandResponse response = new () {Succeeded = result.Succeeded };

            if (result.Succeeded)

                response.Message = "Kullanıcı başarıyla oluşturulmuştur.";
            else
            
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code}-{error.Description}\n";
            //response.Message += $"{error.Code}-{error.Description}<br>";
            return response;
            //throw new UserCreateFailedException();
        }
    }
}
