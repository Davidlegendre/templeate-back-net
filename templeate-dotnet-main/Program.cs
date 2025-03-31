using ApiIntegraciones_v2.Filters;
using Application.Services.LOG;
using Domain.Interfaces;
using Domain.Interfaces.Repository.LOG;
using Domain.Interfaces.Repository.Parametros;
using Domain.Interfaces.Services.LOG;
using Infraestructure.BDConfig;
using Infraestructure.Repository.LOG;
using Infraestructure.Repository.Parametro;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using templeate_dotnet_main.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region INI_Builder
var rutaComplete = $"C:/inetpub/cnn/config_bd.ini";

if (!File.Exists(rutaComplete)) rutaComplete = $"{Environment.CurrentDirectory}/config_bd.ini";

builder.Configuration.AddIniFile(rutaComplete, optional: false, reloadOnChange: true);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = templeate_dotnet_main.Handlers.Utils.GetTokenValidationParameters(builder.Configuration);
});

builder.Services.AddSingleton<IBDConfig, BDConfiguration>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APi X",
        Version = "v2",
        License = new OpenApiLicense
        {
            Name = "Copyright © 2024. Todos los derechos reservados. Desarrollado por X"
        }
    });
    /*
      para el candadito global y por cada ruta, dependiendo si la ruta es AllowAnonymous o tiene Authorized
     */
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer <Token>\"",
    });
    //aqui construye el candadito para cada ruta
    c.OperationFilter<AuthorizeCheckOperationFilter>();
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    c.DocInclusionPredicate((name, api) => true);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IParametrosRepository, ParametroRepository>();
builder.Services.AddScoped<ILOGRepository, LOGRepository>();
builder.Services.AddScoped<ILOGService, LOGService>();

var app = builder.Build();

var cultureInfo = new CultureInfo("es-PE");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
var supportedCultures = new[] { cultureInfo };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerServer>();
app.UseMiddleware<JwtMiddlewareHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
