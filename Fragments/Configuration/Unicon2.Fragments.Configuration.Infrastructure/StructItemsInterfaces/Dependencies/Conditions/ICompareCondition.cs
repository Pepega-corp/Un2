using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions
{
    public interface ICompareCondition : ICondition
    {
        ConditionsEnum ConditionsEnum { get; set; }
        ushort UshortValueToCompare { get; set; }
    }
}