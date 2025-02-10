using ChronotrackService.Application;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChronotrackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtraHoursController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public ExtraHoursController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

      

    }
}
