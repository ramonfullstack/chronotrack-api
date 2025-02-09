using ChronotrackService.Application;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(new { token, user });
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

        [HttpPost("get_user")]
        public async Task<IActionResult> GetuserByEmail([FromRoute] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Model não pode ser nulo.");
            }

            try
            {
                var user = await _authService.AuthenticateAsync(email);
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
    }
}
