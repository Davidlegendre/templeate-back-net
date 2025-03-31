using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Parametros
{
    public class ParametrosEntity
    {
        public long ID { get; set; }
        public string SISTEMA_O_PROCESO {  get; set; }
        public string API_URL { get; set; }
        public string DESCRIPCION { get; set; }
        public bool Activo {  get; set; }
        public string BASE_64_CREDENTIALS { get; set; }
    }
}
