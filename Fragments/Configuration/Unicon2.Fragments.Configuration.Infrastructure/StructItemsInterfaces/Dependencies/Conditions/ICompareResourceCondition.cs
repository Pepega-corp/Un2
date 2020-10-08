using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions
{
    public interface ICompareResourceCondition : ICondition
    {
        string ReferencedPropertyResourceName { get; set; }
        ushort UshortValueToCompare { get; set; }
        ConditionsEnum ConditionsEnum { get; set; }

    }
}