using Domain.GlobalModels;
using Domain.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiIntegraciones_v2.Controllers.Logistica
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(422, Type = typeof(LogModel<object?, UnprocessableEntityStatus>))]
    [ProducesResponseType(400, Type = typeof(LogModel<object?, BadRequestStatus>))]
    [ProducesResponseType(500, Type = typeof(LogModel<object?, InternalServerErrorStatus>))]
    [ProducesResponseType(401, Type = typeof(LogModel<object?, UnauthorizedStatus>))]
    [ApiExplorerSettings(GroupName = "grupo para swagger")]
    public class EjemploController : ControllerBase
    {
        readonly IValidator<object> _validator; //inyecciones
        //IArticuloService _service;
        public EjemploController(IValidator<object> validator/*, IArticuloService articuloService*/)
        {
            _validator = validator;
            //_service = articuloService;
        }


        [HttpPost("CreateOrUpdateX")] 
        [ProducesResponseType(200, Type = typeof(LogModel<object, StatusHttpModel>))]//para documentacion Object puede ser una clase
        public async Task<IActionResult> CreateOrUpdateX([FromBody] object articulo)
        {
            //validaciones
            var resultValid = await _validator.ValidateAsync(articulo);
            if (!resultValid.IsValid) return UnprocessableEntity(new LogModel<string, StatusHttpModel>()
            { Status = new UnprocessableEntityStatus() { message = resultValid.Errors.ToConcatErrorsMessage() } });

            //await _service.CreateOrUpdateEntity(articulo); ejecuta el servicio

            //retorna OK
            return Ok(new LogModel<object, StatusHttpModel>()
            {
                data = articulo
            });
        }
    }
}
