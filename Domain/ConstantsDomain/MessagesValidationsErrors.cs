namespace Domain.ConstantsDomain
{
    internal static class MessagesValidationsErrors
    {
        public static string ENTIDAD = "{entidad}";
        public static string NUMBER = "{number}";
        public static string NUMBER2 = "{number2}";

        public static string NO_NULL_OR_VOID_MESSAGE = "{entidad} no puede ser nulo o vacio";
        public static string NO_NULL_MESSAGE = "{entidad} no puede ser nulo";
        public static string NO_NULL_EMAIL_MESSAGE = "{entidad} no puede ser nulo y debe ser un Email Valido";
        public static string VALID_EMAIL = "{entidad} debe ser un Email Valido";
        public static string MAYOR_A_MESSAGE = "{entidad} no puede ser nulo y debe ser mayor a {number}";
        public static string MAYOR_O_IGUAL_A_MESSAGE = "{entidad} no puede ser nulo y debe ser mayor o igual a {number}";
        public static string NO_NULL_OR_EMPTY_AND_NUMBER = "{entidad} no puede ser nulo y debe ser numero";
        public static string LENGTH_MIN_MAX = "{entidad} debe tener {number} caracteres";
        public static string LENGTH_MIN_MAX_BETWEEN = "{entidad} debe estar entre {number} y {number2}";
    }
}
