using Dapper;
using Domain.ConstantsDomain;
using Domain.Entities.LOG;
using Domain.Interfaces;
using Domain.Interfaces.Repository.LOG;
using Microsoft.Data.SqlClient;
using static Domain.ConstantsDomain.Enums;

namespace Infraestructure.Repository.LOG
{
    public class LOGRepository : ILOGRepository
    {
        IBDConfig _config;
        public LOGRepository(IBDConfig bDConfig)
        {
            _config = bDConfig;
        }
        public async Task CreateLog(LogEntity log)
        {
            //se puede alternar de base de datos
            using (var con = new SqlConnection(_config.GetConnectionString(TypeBDs.BD1)))
            {
                await con.OpenAsync();
                var result = await con.ExecuteAsync(ProceduresConstants.BO_CREATE_LOG_EQUIVALENCE,
                    new {
                        @ID_ENTIDAD = log.ID_ENTIDAD,
                        @ID_EVENTO= log.ID_EVENTO,
                        @MENSAJE = log.MENSAJE,
                        @IDENTIFICADOR = log.IDENTIFICADOR
                    }, commandType: System.Data.CommandType.StoredProcedure);
                await con.CloseAsync();               
            }
        }
    }
}
