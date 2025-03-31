using Domain.Entities.Parametros;

namespace Domain.Interfaces.Repository.Parametros
{
    public interface IParametrosRepository
    {
        public Task<ParametrosEntity?> GetParametro(long typeApiParametros);
    }
}
