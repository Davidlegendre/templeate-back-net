using Domain.GlobalModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace templeate_dotnet_main.Handlers
{
    /// <summary>
    /// Maneja Todas las Excepciones y todos los status para luego enviar un objeto json personalizado
    /// Apartir de ahora, no es necesario TryCatch (solo para casos especiales)
    /// </summary>
    public class ExceptionHandlerServer
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerServer> _logger;        
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlerServer(RequestDelegate next, ILogger<ExceptionHandlerServer> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            /*
                si no hay una excepcion, toma el StatusCode para generar la respuesta
            si hay una excepcion le pasa la excepcion adicionalmente
             */

            var body = await GetRequestBody(context);
            var originalBodyStream = context.Response.Body;

            bool IsBodyChange = false;

            // Usar un MemoryStream para capturar la respuesta
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                try
                {
                    await _next(context);
                    ///genera el log segun el status
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    var responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await HandleExceptionAsync(context, context.Response.StatusCode, ref IsBodyChange, body, bodyResponse: responseContent);

                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, context.Response.StatusCode, ref IsBodyChange, body, ex: ex);
                    //genera el log segun la excepcion
                }
                finally
                {
                    // Si HandleExceptionAsync ha modificado el cuerpo de la respuesta, lo usamos
                    if (IsBodyChange)
                    {
                        // Asegurarse de leer el contenido de la respuesta y escribirlo en el flujo original
                       
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        var modifiedResponseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
                        context.Response.Body = originalBodyStream;  // Restaurar el flujo original
                        await new StreamWriter(context.Response.Body).WriteAsync(modifiedResponseContent);  // Escribir la respuesta modificada
                        await context.Response.WriteAsync(modifiedResponseContent);
                    }
                    else
                    {
                        // Si no se ha modificado, solo copiar el contenido procesado al cuerpo original
                        context.Response.Body.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                   
                    //// Restaurar el flujo original de la respuesta
                    //context.Response.Body.Seek(0, SeekOrigin.Begin);
                }
            }
        }

        private Task HandleExceptionAsync(HttpContext context, int statusCode, ref bool IsBodyChange, string body = "", string bodyResponse = "", Exception? ex = null) {

            var msgEx = context.GetEndpoint() == null ? "Endpoint No encontrado": "";
            var statusHttp = ((HttpStatusCode)statusCode);

            msgEx = statusHttp == HttpStatusCode.MethodNotAllowed ? $"El Endpoint no tiene ese Method: {context.Request.Method}" : msgEx;

            if (ex != null && ex.Message.Contains("DBCC"))
            {
                var mensajes = ex.Message.Split("\r\n").Where(e => !e.Contains("Comprobación de información de identidad") && !e.Contains("DBCC"));
                if (mensajes.Count() > 0)
                {
                    msgEx = string.Join(" ", mensajes);
                }
            }
            else if(ex != null)
                msgEx = ex.Message;

            if (!string.IsNullOrWhiteSpace(bodyResponse))
            { 
               JObject json = JObject.Parse(bodyResponse);

                if (json != null && json.Property("errors") != null || statusHttp == HttpStatusCode.UnsupportedMediaType)
                {
                    LogModel<object, StatusHttpModel>? log1 = new LogModel<object, StatusHttpModel>()
                    {
                        Status = new StatusHttpModel()
                        {
                            isError = true,
                            Status = (int)statusHttp,
                            StatusString = statusHttp.ToString()
                        }
                    };
                    // Obtener el objeto 'errors'
                    if (statusHttp == HttpStatusCode.UnsupportedMediaType) {
                        log1.Status.message = "El Tipo de Medio no es Compatible o no se proporciono un body";
                        bodyResponse = JsonConvert.SerializeObject(log1);
                        IsBodyChange = true;
                    } else {
                        var errors = json["errors"];

                        // Verificar que 'errors' es un objeto
                        if (errors!.Type == JTokenType.Object)
                        {
                            JObject errores = (JObject)errors;
                            var props = errores.Properties().ToList();

                            props.ForEach(x =>
                            {
                                if (x.Value.Type == JTokenType.Array)
                                {
                                    var value = x.Value;
                                    msgEx += (!string.IsNullOrWhiteSpace(x.Name) ? $"[{x.Name}] " : "") + string.Join(" | ", value);
                                    msgEx += " | ";
                                }
                            });
                            log1.Status.message = msgEx.Remove(msgEx.LastIndexOf(" | "));
                            bodyResponse = JsonConvert.SerializeObject(log1);
                            IsBodyChange = true;
                        }
                    }
                }
            }

         
            /*
                cuando hay una excepcion el statuscode es 200, es por eso que cambia el status
            dependiendo de la excepcion, mediante un switch abreviado
             */
            if (ex != null)
                statusHttp = ex switch
                {
                    FluentValidation.ValidationException
                        => HttpStatusCode.UnprocessableEntity,

                    FormatException or ArgumentException or JsonException or ValidationException or ArgumentNullException
                    or InvalidOperationException or KeyNotFoundException or SqlException
                        => HttpStatusCode.BadRequest,

                    UnauthorizedAccessException or SecurityTokenExpiredException or
                    Microsoft.IdentityModel.Tokens.SecurityTokenInvalidLifetimeException
                        => HttpStatusCode.Unauthorized,

                    NotSupportedException => HttpStatusCode.MethodNotAllowed,

                    _ => HttpStatusCode.InternalServerError
                };


            //crea el log a partir de un status
            /*
                este es el objeto con el que se muestra al usuario de la api
             */
      
            LogModel<object, StatusHttpModel>? log = string.IsNullOrWhiteSpace(bodyResponse) ? new LogModel<object, StatusHttpModel>()
            {
                Status = new StatusHttpModel()
                {
                    isError = true,
                    Status = (int)statusHttp,
                    message = ex != null ? (statusHttp == HttpStatusCode.InternalServerError ? "Verifique la consola": msgEx) : msgEx,
                    StatusString = statusHttp.ToString()
                }
            }: null;

           

            /*
                imprime el log en la consola para informacion de todas las peticiones que existen
            junto a sus mensajes, si hay alguno
             *
             * ENVIAR A UN TXT AL SERVER, ADEMAS DE MODIFICAR EL LOG PARA REGISTRAR EN LA BD
             * Y TAMBIEN REGISTRAR A UNA TABLA DE ENVIO DE EMAILS, UNO POR PETICION, BORRANDO EL ANTERIOR,
             * LA ESTRUCTURA PARA ENVIAR AL TXT ES EL MISMO DEL LOG SINO QUE AÑADIENDO EL BODY,
             * ADEMAS DE PONER EL IP DEL QUE ENVIO LA PETICION,
             * TENER VAR GLOBAL PARA LA URL DE LA CARPETA
             */
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                 ?? context.Connection.RemoteIpAddress?.ToString();

            
            var headers = GetHeaders(context.Request.Headers);

            var IgnoreEndpointsToLog = new List<string>()
            {
                "f9a1c7b4-49cf-4a1d-8f71-2e3fd5b58f76",
                "f9a1c7b4-49cf-4a1d-8f71-2e3fd5b58f77",
                "Test"
            };



            if (!IgnoreEndpointsToLog.Contains(context.Request.Path.Value!.Split("/").Last()))
            {

                var msg = @$"{(statusHttp != HttpStatusCode.OK ? "Ocurrió un error en:" : "Respuesta de:")}
###Fecha: {DateTime.Now}
###IP: {ip}
###Host: {context.Request.Host}
###Method: {context.Request.Method}
###ContentType: {context.Request.ContentType}
###Endpoint: {context.Request.Path}
###Schema: {context.Request.Scheme.ToUpper()}
###Header: {headers.Trim()}
###Status: {(int)statusHttp}-{statusHttp.ToString()}
{(ex != null ? $@"###Excepcion: {ex.GetType()}
###Mensaje: {ex.Message}
            {ex.StackTrace}" : "").Trim()}            
###BODY_REQUEST: {body}
###BODY_RESPONSE:{(log != null ? JsonConvert.SerializeObject(log) : bodyResponse)}";

                if (statusHttp == HttpStatusCode.InternalServerError) _logger.LogError(msg + Environment.NewLine);/* else _logger.LogInformation(msg + Environment.NewLine);*/

                Utils.CreateLogTXT(_env.ContentRootPath, msg);

            }

            /*
                para los 415 Unsupported Media Type, no se puede modificar el contentType ni
            el StatusCode, ya que los headers estan en modo lectura, para eso solo imprimo lo que
            el server me da, mas no envio el personalizado
             */
            var notreadonlyresponse = !context.Response.Headers.IsReadOnly;
            if (notreadonlyresponse)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusHttp;
            }
            context.Response.Body.SetLength(0);

            return log != null ? context.Response.WriteAsJsonAsync(log)
                : context.Response.WriteAsync(bodyResponse);
        }

        private async Task<string> GetRequestBody(HttpContext context) {
            context.Request.EnableBuffering();

            // Leer el cuerpo como un string
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                string cuerpo = await reader.ReadToEndAsync();

                // Reestablecer la posición del cuerpo para que se pueda leer nuevamente
                context.Request.Body.Position = 0;
                try
                {
                    cuerpo = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(cuerpo), Formatting.None);
                }
                catch
                {
                    cuerpo = "{}";
                }
                return cuerpo; // Devuelve el cuerpo como respuesta
            }
        }

        private string GetHeaders(IHeaderDictionary headers)
        {
            /*User-Agent, Authorization, Content-Length, sec-ch-ua-platform, sec-ch-ua-mobile, sec-ch-ua, sec-fetch-site*/
            var keys = new string[] { "User-Agent", "Authorization", "Content-Length", "sec-ch-ua-platform", 
            "sec-ch-ua-mobile", "sec-ch-ua", "sec-fetch-site" };
            var headersLog = new StringBuilder();
            foreach (var header in headers)
            {
                if (keys.Contains(header.Key))
                    headersLog.AppendLine($"{header.Key}: {header.Value}");
            }
            return headersLog.ToString();
        }
    }
}
