using Domain.GlobalModels;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace templeate_dotnet_main.Controllers.Globales
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[AllowAnonymous] --global o se puede hacer individual

    public class HealthController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("HealthAllow")]
        [ProducesResponseType(200, Type = typeof(LogModel<string, StatusHttpModel>))] //para el retorno de la documentacion
        public IActionResult HealthAllow()
        {
            return Ok(new LogModel<string, StatusHttpModel>() { Status = new StatusHttpModel(), data = "pong" }); //si es ok, retornar asi con logmodel directamente
        }

    }
}
