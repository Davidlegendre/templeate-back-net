using Domain.Entities.LOG;
using static Domain.ConstantsDomain.Enums;

namespace Domain.Interfaces.Services.LOG
{
    public interface ILOGService
    {
        public Task CreateLog(LogEntity log);
    }
}
