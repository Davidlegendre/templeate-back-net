using System.ComponentModel;

namespace Domain.GlobalModels
{
    public class LogModel<T, TStatus> where TStatus : StatusHttpModel
    {
        public TStatus Status { get; set; } = (TStatus)new StatusHttpModel();
        [DefaultValue(null)]
        public T? data { get; set; } = default(T);
        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
