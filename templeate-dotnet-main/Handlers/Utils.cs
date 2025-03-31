using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace templeate_dotnet_main.Handlers
{
    public static class Utils
    {
        /*
            Para fabricar los parametros del token, ya que lo uso dos veces
         */
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration _configuration)
        {
            var section = _configuration.GetSection("AppSettings");
            string key = section.GetValue<string>("SecretKey")!;

            string encodeKey = Encoding.Default.GetString(Convert.FromBase64String(key));
            string ValidIssuer = section.GetValue<string>("ValidIssuer")!;
            string ValidAudience = section.GetValue<string>("ValidAudience")!;

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = ValidIssuer,
                ValidAudience = ValidAudience,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(encodeKey))
            };
        }

        public static (string msg, bool status) CreateLogTXT(string rutaServer, string mensaje, bool IsSeparator = true)
        {
            try
            {
                var separador = $"{Environment.NewLine}-----|LOG|-----{Environment.NewLine}";
                var date = DateTime.Now;
                string dia = date.Day < 10 ? "0" + date.Day : date.Day.ToString();
                string mes = date.Month < 10 ? "0" + date.Month : date.Month.ToString();

                string fecha = $"{dia}-{mes}-{date.Year}";

                string filePath = rutaServer + "/LOG_TXT/log_" + fecha + ".txt";
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(mensaje);
                    if (IsSeparator)
                        writer.WriteLine(separador);
                }
                return ("", true);
            }
            catch (Exception ex)
            {
                return (ex.Message, false);
            }
        }


        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

        
    }
}
