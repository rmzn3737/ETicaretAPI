using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using MediatR;

namespace ETicaret.Application.Features.Commands.AppUser.PasswordReset
{
    public class PasswordResetCommandHandler:IRequestHandler<PasswordResetCommandRequest, PasswordResetCommandResponse>
    {
        readonly IAuthService _authService;

        public PasswordResetCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<PasswordResetCommandResponse> Handle(PasswordResetCommandRequest request, CancellationToken cancellationToken)
        {
            await _authService.PasswordResetAsync(request.Email);
            return new ();
        }
    }
}
