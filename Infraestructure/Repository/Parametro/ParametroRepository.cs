using Dapper;
using Domain.ConstantsDomain;
using Domain.Entities.Parametros;
using Domain.Interfaces;
using Domain.Interfaces.Repository.Parametros;
using Microsoft.Data.SqlClient;

namespace Infraestructure.Repository.Parametro
{
    public class ParametroRepository : IParametrosRepository
    {
        IBDConfig _config;
        public ParametroRepository(IBDConfig config)
        {
            _config = config;
        }
        public async Task<ParametrosEntity?> GetParametro(long typeApiParametros)
        {
            using (var con = new SqlConnection(_config.GetConnectionString(Domain.ConstantsDomain.Enums.TypeBDs.BD1)))
            { 
                await con.OpenAsync();
                var result = await con.QueryFirstOrDefaultAsync<ParametrosEntity>(ProceduresConstants.BO_OBTENER_URL_PARAMETRO, new
                {
                    @ID_PARAMETRO = typeApiParametros
                }, commandType: System.Data.CommandType.StoredProcedure);
                await con.CloseAsync();
                return result;
            }
        }
    }
}
