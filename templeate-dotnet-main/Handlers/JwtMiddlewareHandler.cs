using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace templeate_dotnet_main.Handlers
{
    public class JwtMiddlewareHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddlewareHandler> _logger;
        private readonly IConfiguration _configuration;

        public JwtMiddlewareHandler(RequestDelegate next, ILogger<JwtMiddlewareHandler> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            /*
                No es necesario colocar tryCatch ya que todo lo maneja el ExceptionHandlerServer             
             */
            var endpoint = context.GetEndpoint();
            if (endpoint == null || endpoint.Metadata.Count() == 0) { await _next(context); return; }
            if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null) { await _next(context); return; }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            /*
             Para activar el ExceptionHandlerServer en alguna parte del codigo de manera perfonalizada, 
            se debe hacer un throw New y la excepcion que se basa, si no existe ira a status 500 pero si se quiere
            personalizar el error, se debe ir a ExceptionHandlerServer y añadirlo
             */
            if (token == null) throw new UnauthorizedAccessException("Authorization no puede estar nulo, debe ser Bearer token");

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = Utils.GetTokenValidationParameters(_configuration);

            // Validar el token
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            }
            catch (SecurityTokenInvalidLifetimeException)
            {
                throw new SecurityTokenInvalidLifetimeException("Tiempo expirado");
            }
            catch {
                throw new UnauthorizedAccessException("Token Invalido");
            }

            await _next(context);
        }
    }

}
