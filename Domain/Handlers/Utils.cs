using FluentValidation.Results;
using static Domain.ConstantsDomain.Enums;

namespace Domain.Handlers
{
    public static class Utils
    {
        public static TypeEventoLog GetEvento(int ID)
        {
            var result = Enum.GetValues(typeof(TypeEventoLog)).GetValue(ID);
            if (result == null) result = TypeEventoLog.En_Curso;
            return (TypeEventoLog)result;
        }


        /// <summary>
        /// obtiene una plantilla de MessagesvalidationsErrors y mediante los params le asigna los valores a la plantilla
        /// </summary>
        /// <param name="mensaje">Plantilla</param>
        /// <param name="values">Valores a reemplazar</param>
        /// <returns>Mensaje completo</returns>
        public static string Mensaje(string mensaje, params (string part, string value)[] values) {
            foreach (var (part, value) in values)
            {
                mensaje = mensaje.Replace(part, value); 
            }
            return mensaje; 
        }

        public static string ToConcatErrorsMessage(this List<ValidationFailure> lista) {
            return string.Join(" | ",lista.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
        }
       public static string? GetBase64String(string? db)
        {
            if (string.IsNullOrWhiteSpace(db)) return null;

            byte[] bytes = Convert.FromBase64String(db);
            // Convertir bytes a string
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

    }
}
