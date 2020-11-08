using Unicon2.Infrastructure.Dependencies;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies
{
    public interface IConditionResultDependency : IDependency
    {
        ICondition Condition { get; set; }
        IDependencyResult Result { get; set; }     
    }
}