using System.ComponentModel;

namespace Domain.GlobalModels
{
    /*
     hay varias clases que heredan de este pero solo son para la documentacion del Swagger, realmente
    no se usan, ya que este el global para todos
     */
    public class StatusHttpModel
    {
        [DefaultValue("")]
        public string message { get; set; } = string.Empty;
        [DefaultValue(false)]
        public virtual bool isError { get; set; } = false;

        [DefaultValue(200)]
        public virtual int Status { get; set; } = 200;

        public virtual string StatusString { get; set; } = "OK";
    }
}
