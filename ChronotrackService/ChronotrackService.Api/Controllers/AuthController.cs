using ChronotrackService.Application;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChronotrackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model == null)
            {
                return BadRequest("Model não pode ser nulo.");
            }

            try
            {
                var (token, user) = await _authService.AuthenticateAsync(model.Email, model.Password);
                return Ok(
                    new { 
                        token, 
                        user,
                        message = $"Logado com sucesso {user.Name}"
                    });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Falha na autenticação.");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a autenticação.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("getuser")]
        public async Task<IActionResult> GetuserByEmail([FromRoute] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Model não pode ser nulo.");
            }

            try
            {
                var user = await _authService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Falha na autenticação.");
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a autenticação.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest("Refresh token não pode ser nulo.");
            }

            try
            {
                var (accessToken, newRefreshToken) = await _authService.RefreshTokenAsync(model.RefreshToken);

                // Retorna o novo access token e o novo refresh token
                return Ok(new { accessToken, refreshToken = newRefreshToken });
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogError(ex, "Refresh token inválido ou expirado.");
                return Unauthorized(new { message = "Refresh token inválido ou expirado." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar o refresh token.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}

