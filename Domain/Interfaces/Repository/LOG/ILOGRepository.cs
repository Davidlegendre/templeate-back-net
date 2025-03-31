using Domain.Entities.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.ConstantsDomain.Enums;

namespace Domain.Interfaces.Repository.LOG
{
    public interface ILOGRepository
    {
        public Task CreateLog(LogEntity log);
    }
}
