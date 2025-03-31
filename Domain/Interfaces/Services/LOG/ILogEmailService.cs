using Domain.Entities.LOG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.LOG
{
    public interface ILogEmailService
    {
        public Task<string> CreateHTML();
    }
}
