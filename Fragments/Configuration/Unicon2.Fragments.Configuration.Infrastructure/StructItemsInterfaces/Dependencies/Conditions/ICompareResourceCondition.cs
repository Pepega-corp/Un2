using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions
{
    public interface ICompareResourceCondition :  ICompareCondition
    {
        string ReferencedPropertyResourceName { get; set; }

    }
}