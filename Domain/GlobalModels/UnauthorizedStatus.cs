using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class UnauthorizedStatus : StatusHttpModel
    {
        [DefaultValue(true)]
        public override bool isError { get; set; } = true;
        [DefaultValue(401)]
        public override int Status { get; set; } = 401;
        public override string StatusString { get; set; } = "Unauthorized";
    }
}
