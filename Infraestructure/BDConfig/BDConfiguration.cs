using Dapper;
using Domain.Handlers;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using static Domain.ConstantsDomain.Enums;
using System.Linq;

namespace Infraestructure.BDConfig
{
    public class BDConfiguration : IBDConfig
    {
        IConfiguration _config;
        public BDConfiguration(IConfiguration configuration)
        {
            _config = configuration;
        }
        /// <summary>
        /// cambia segun lo indicado en el appsettings.json
        /// </summary>
        /// <returns></returns>
        //public string? GetConnectionString(TypeBDs typeBDs)
        //{
        //    var EsServerDev = _config.GetValue<bool>("EsServerDev");

        //    var sectionDev = _config.GetSection("Dev");
        //    var sectionProd = _config.GetSection("Prod");

        //    string? bd = EsServerDev ?
        //        sectionDev.GetSection(typeBDs.ToString()).Value : 
        //        sectionProd.GetSection(typeBDs.ToString()).Value;

        //    return GetBase64String(bd) ?? "";
        //}
        
        public string? GetConnectionString(TypeBDs typeBDs)
        {
            var EsServerDev = _config.GetValue<short>("Connection_DB:EsServerDev");
            var sectionDev = _config.AsEnumerable().Where(x => x.Key.StartsWith("Connection_DB:Dev_")).AsList();
            var sectionProd = _config.AsEnumerable().Where(x => x.Key.StartsWith("Connection_DB:Prod_")).AsList();

            string? bd = EsServerDev == 1 ?
                sectionDev.FirstOrDefault(x => x.Key.Equals($"Connection_DB:Dev_{typeBDs.ToString()}")).Value :
                sectionProd.FirstOrDefault(x => x.Key.Equals($"Connection_DB:Prod_{typeBDs.ToString()}")).Value;

            return Utils.GetBase64String(bd) ?? "";
        }
    }
}
