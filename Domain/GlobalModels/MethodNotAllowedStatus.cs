using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class MethodNotAllowedStatus : StatusHttpModel
    {
        [DefaultValue(true)]
        public override bool isError { get; set; } = true;
        [DefaultValue(405)]
        public override int Status { get; set; } = 405;
        public override string StatusString { get; set; } = "MethodNotAllowed";
    }
}
