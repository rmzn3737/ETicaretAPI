using ETicaret.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaret.Application.Features.Commands.AppUser.LoginUser;
using ETicaret.Application.Features.Commands.AppUser.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody]RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<ActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse googleLoginCommandResponse = await _mediator.Send(googleLoginCommandRequest);
            return Ok(googleLoginCommandResponse);
        }

        [HttpPost("facebook-login")]
        public async Task<ActionResult> FacebookLogin(FacebookLoginCommandRequest facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse facebookLoginCommandResponse = await _mediator.Send(facebookLoginCommandRequest);
            return Ok(facebookLoginCommandResponse);
        }
    }
}
