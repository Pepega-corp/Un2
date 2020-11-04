using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters
{
    public interface IFilter
    {
        string Name { get; set; }
        ICompareResourceCondition
    }
}