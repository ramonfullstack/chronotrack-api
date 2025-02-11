using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChronotrackService.Application
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public ChronotrackContext ChronotrackContext { get; }

        public AuthService(IUserRepository userRepository,
            IConfiguration configuration,
            ChronotrackContext chronotrackContext)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            ChronotrackContext = chronotrackContext;
        }

        public async Task<(string, UserEntity)> AuthenticateAsync(string email, string password)
        {
            var user = await ChronotrackContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);

            if (user == null || user.Password != password)
            {
                throw new UnauthorizedAccessException("Email ou senha incorretos");
            }

            var token = GenerateJwtToken(user);
            return (token, user);
        }


        public async Task<UserEntity> GetUserByEmail(string email)
        {
            return await ChronotrackContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Expira em 1 hora
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string AccessToken, string NewRefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // Verifica se o refresh token existe e ainda não expirou
            var user = await ChronotrackContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                throw new SecurityTokenException("Refresh token inválido ou expirado.");
            }

            // Gera um novo access token
            var accessToken = GenerateJwtToken(user);

            // Gera um novo refresh token
            var newRefreshToken = GenerateRefreshToken();

            // Atualiza o refresh token no banco de dados
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Renova a expiração para 7 dias
            user.Updated = DateTime.Now;
            await ChronotrackContext.SaveChangesAsync();

            return (accessToken, newRefreshToken);
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await ChronotrackContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                throw new ArgumentException("Usuário não encontrado.");
            }

            // Atualiza o refresh token e a data de expiração
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Expira em 7 dias
            user.Updated = DateTime.UtcNow;

            await ChronotrackContext.SaveChangesAsync();
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32]; // 32 bytes = 256 bits
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber); // Converte para uma string Base64
        }

    }
}
