using ETicaret.Application.Abstractions.Services;
using MediatR;

namespace ETicaret.Application.Features.Commands.AppUser.GoogleLogin
{
    public class GoogleLoginCommandHandler:IRequestHandler<GoogleLoginCommandRequest,GoogleLoginCommandResponse>
    {
        /*readonly UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;

        public GoogleLoginCommandHandler(UserManager<ETicaretAPI.Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }*/
        readonly IAuthService _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
            var token= await _authService.GoogleLoginAsync(request.IdToken, 900);////15 dk lık JWT oluşturuyoruz.
            return new()
            {
                Token = token
            };
            /*var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    "394522923487-gmcqd6gsp8ja657ic35pv3711md6k44p.apps.googleusercontent.com"
                }
                //Audience = new List<string> { "394522923487-gmcqd6gsp8ja657ic35pv3711md6k44p.apps.googleusercontent.com" }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken,settings);
            var info= new UserLoginInfo(request.Provider, payload.Subject,request.Provider);
            ETicaretAPI.Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user ==null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        NameSurname = payload.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result)
                await _userManager.AddLoginAsync(user, info);//AspNetUserLogins tablosuna kullanıcıyı ekledik.
            else
                throw new Exception("Invalid external authentication.");

            Token token = _tokenHandler.CreateAccessToken(30);

            return new()
            {
                Token = token
            };*/
        }
    }
}
