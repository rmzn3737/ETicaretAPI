using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.DTOs.User;
using ETicaretAPI.Domain.Entities.Identity;

namespace ETicaret.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
    }
}
