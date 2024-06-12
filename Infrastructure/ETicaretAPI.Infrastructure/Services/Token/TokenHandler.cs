using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Token;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ETicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ETicaret.Application.DTOs.Token CreateAccessToken(int second,AppUser user)
        {
            ETicaret.Application.DTOs.Token token = new();
            //SecurityKey in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarlarını veriyoruz.
            token.Expiration = DateTime.UtcNow.AddSeconds(second);
            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                //expires: DateTime.UtcNow.AddMinutes(minute).AddHours(3),
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow, //Üretildiği anda devreye girsin.
                signingCredentials: signingCredentials,
                claims: new List<Claim> { new(ClaimTypes.Name, user.UserName) }
                );
            

            //Token oluşturucu sınıfından bir örnek alalım.
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken(securityToken);

            //string refreshToken = CreateRefreshToken();
            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public string CreateRefreshToken()
        {
            byte[]  number = new byte[32];

            /*using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                
            }*/ //todo using eski kullanım, Engin hoca anlatmıştı, Gençay Hoca da scope dan çıkılınca nesne dipos edilecek  diye belirtti. Yani grbage collector toplayacak, imha edecek. Yeni kullanım altta.

            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(number);

            return Convert.ToBase64String(number);

        }
    }
}


//*******************//
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ETicaret.Application.Abstractions.Token;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace ETicaretAPI.Infrastructure.Services.Token
//{
//    public class TokenHandler : ITokenHandler
//    {
//        readonly IConfiguration _configuration;

//        public TokenHandler(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public ETicaret.Application.DTOs.Token CreateAccessToken(int minute)
//        {
//            ETicaret.Application.DTOs.Token token = new();
//            //SecurityKey in simetriğini alıyoruz.
//            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

//            //Şifrelenmiş kimliği oluşturuyoruz.
//            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

//            //Oluşturulacak token ayarlarını veriyoruz.
//            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
//            JwtSecurityToken securityToken = new(
//                audience: _configuration["Token:Audience"],
//                issuer: _configuration["Token:Issuer"],
//                expires: token.Expiration,
//                notBefore: DateTime.UtcNow, //Üretildiği anda devreye girsin.
//                signingCredentials: signingCredentials);

//            //Token oluşturucu sınıfından bir örnek alalım.
//            JwtSecurityTokenHandler tokenHandler = new();
//            token.AccessToken = tokenHandler.WriteToken(securityToken);
//            return token;
//        }
//    }
//}

