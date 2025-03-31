using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class UnprocessableEntityStatus : StatusHttpModel
    {
        [DefaultValue(true)]
        public override bool isError { get; set; } = true;
        [DefaultValue(422)]
        public override int Status { get; set; } = 422;
        public override string StatusString { get; set; } = "UnprocessableEntity";

    }
}
