using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.DTOs.User;
using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler:IRequestHandler<CreateUserCommandRequest,CreateUserCommandResponse>
    {
        readonly IUserService _userService;
        //private readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> userManager, IUserService userService)
        {
            _userService = userService;
            //_userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            /*IdentityResult result = await _userManager.CreateAsync(new()
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
            //response.Message += $"{error.Code}-{error.Description}<br>";*/

            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                UserName = request.UserName,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm
            });
            return new ()
            {
                Message = response.Message,
                Succeeded = response.Succeeded
            };
            //throw new UserCreateFailedException();
        }
    }
}
