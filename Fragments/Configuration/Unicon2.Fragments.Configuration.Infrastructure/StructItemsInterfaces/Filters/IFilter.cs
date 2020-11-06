using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters
{
    public interface IFilter
    {
        string Name { get; set; }
        ICondition Condition { get; set; }
    }
}