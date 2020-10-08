using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies
{
    public interface IConditionResultDependency : IDependency
    {
        ICondition Condition { get; set; }
        IDependencyResult Result { get; set; }     
    }
}