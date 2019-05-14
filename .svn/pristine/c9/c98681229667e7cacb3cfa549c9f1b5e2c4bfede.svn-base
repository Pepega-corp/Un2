using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties
{
    public interface IDependentProperty : IProperty
    {
        List<IDependancyCondition> DependancyConditions { get; set; }
        ConditionResultEnum ActualConditionResult { get; set; }
        
        IUshortsFormatter DeviceValueUshortsFormatter { get; set; }
        IUshortsFormatter LocalValueUshortsFormatter { get; set; }

    }
}