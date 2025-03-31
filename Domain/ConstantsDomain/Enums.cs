namespace Domain.ConstantsDomain
{
    public class Enums
    {
        public enum TypeBDs
        {
            BD1,
            BD2
        }

        public enum TypeEventoLog
        { 
            En_Curso = 1,
            Aceptado = 2,
            Cancelado = 3
        }

        /// <summary>
        /// si se quiere hacer una tabla con los parametros de cada api
        /// ejemplo:
        /// ID 2 | Nombre: "Parametro1" | Valor: "http://www.api-externa-o-interna/api" | Descripcion: "Descripcion1"
        /// </summary>
        public enum TypeApiParametros
        {
            
        }
    }
}
