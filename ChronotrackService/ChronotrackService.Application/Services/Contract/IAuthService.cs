using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ChronotrackService.Application
{
    public interface IAuthService
    {
        Task<(string, UserEntity)> AuthenticateAsync(string email, string password);
        Task<UserEntity> GetUserByEmail(string email);
        Task SaveRefreshTokenAsync(int userId, string refreshToken);

        Task<(string AccessToken, string NewRefreshToken)> RefreshTokenAsync(string refreshToken);



    }
}
