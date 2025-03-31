using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class InternalServerErrorStatus : StatusHttpModel
    {
        [DefaultValue(true)]
        public override bool isError { get; set; } = true;
        [DefaultValue(500)]
        public override int Status { get; set; } = 500;
        public override string StatusString { get; set; } = "InternalServerError";
    }
}
