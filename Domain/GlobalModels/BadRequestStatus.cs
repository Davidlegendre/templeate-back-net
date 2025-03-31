using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class BadRequestStatus : StatusHttpModel
    {
        [DefaultValue(true)]
        public override bool isError { get; set; } = true;
        [DefaultValue(400)]
        public override int Status { get; set; } = 400;
        public override string StatusString { get; set; } = "BadRequest";
    }
}
