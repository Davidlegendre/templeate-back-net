using static Domain.ConstantsDomain.Enums;

namespace Domain.Interfaces
{
    public interface IBDConfig
    {
        public string? GetConnectionString(TypeBDs typeBDs);
    }
}
