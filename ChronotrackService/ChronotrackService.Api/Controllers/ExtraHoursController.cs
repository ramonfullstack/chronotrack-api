using ChronotrackService.Application;
using Microsoft.AspNetCore.Mvc;

namespace ChronotrackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtraHoursController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public IExtraHourService _service { get; }

        public ExtraHoursController(ILogger<AuthController> logger, 
            IExtraHourService service)
        {
            _logger = logger;
            _service = service;
        }


        [HttpGet("getHoursWorkedByUser/{idUser}")]
        public async Task<ActionResult<List<ExtraHoursEntity>>> GetExtraHoursByUser(int idUser)
        {
            var extraHours = await _service.GetExtraHoursByUserAsync(idUser);

            if (extraHours == null || extraHours.Count == 0)
            {
                return NotFound($"Não foram encontradas horas extras para o usuário com ID {idUser}.");
            }

            return Ok(extraHours);
        }

        [HttpPost("saveHour")]
        public async Task<ActionResult<ExtraHoursEntity>> SaveOrUpdateExtraHour([FromBody] ExtraHoursEntity extraHour)
        {
            if (extraHour == null)
            {
                return BadRequest("A hora extra fornecida é inválida.");
            }

            var savedExtraHour = await _service.SaveOrUpdateExtraHourAsync(extraHour);

            return CreatedAtAction(nameof(GetExtraHourById), new { id = savedExtraHour.Id }, savedExtraHour);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExtraHoursEntity>> GetExtraHourById(int id)
        {
            var extraHour = await _service.GetExtraHourByIdAsync(id);

            if (extraHour == null)
            {
                return NotFound($"Não foi encontrada hora extra com o ID {id}.");
            }

            return Ok(extraHour);
        }

    }
}
