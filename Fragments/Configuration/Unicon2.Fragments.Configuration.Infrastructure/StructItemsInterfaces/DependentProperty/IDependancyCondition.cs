using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty
{
    public interface IDependancyCondition : IUshortFormattable
    {
	    /// <summary>
        /// Ссылаемый ресурс
        /// </summary>
        string ReferencedPropertyResourceName { get; set; }
        ConditionsEnum ConditionsEnum { get; set; }
        ushort UshortValueToCompare { get; set; }
        ConditionResultEnum ConditionResult { get; set; }

       
    }

}