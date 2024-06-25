using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.Features.Commands.AppUser.CreateUser;
using ETicaret.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaret.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaret.Application.Features.Commands.AppUser.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        readonly IMailService _mailService;
        public UsersController(IMediator mediator, IMailService mailService)
        {
            _mediator = mediator;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserCommandRequest _createUserCommandRequest)
        {
            CreateUserCommandResponse createUserCommandResponse = await _mediator.Send(_createUserCommandRequest);
            return Ok(createUserCommandResponse);
        }

        [HttpGet]
        public async Task<IActionResult> ExampleMailTest()
        {
            await _mailService.SendMessageAsync("onlineogretmen43@gmail.com", "Örnek Mail", "<strong>Bu bir örnek maildir.</strong>");
            return Ok();
        }


    }

    //[HttpPost("[action]")]
    //public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
    //{
    //    LoginUserCommandResponse loginUserCommandResponse = await _mediator.Send(loginUserCommandRequest);
    //    return Ok(loginUserCommandResponse);
    //}
}

