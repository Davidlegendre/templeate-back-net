using Domain.Entities.LOG;
using Domain.Interfaces.Repository.LOG;
using Domain.Interfaces.Services.LOG;
using System.Text;
using static Domain.ConstantsDomain.Enums;

namespace Application.Services.LOG
{
    public class LOGService : ILOGService
    {
        ILOGRepository _log;
        public LOGService(ILOGRepository log)
        {
            _log = log;
        }

        public async Task CreateLog(LogEntity log) => await _log.CreateLog(log);
    }
}
