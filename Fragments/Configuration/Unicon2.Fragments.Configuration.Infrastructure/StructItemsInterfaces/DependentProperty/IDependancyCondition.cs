using System;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty
{
    public interface IDependancyCondition : IUshortFormattable
    {

        /// <summary>
        /// Ссылаемое свойство
        /// </summary>
       // ILocalAndDeviceValuesContaining LocalAndDeviceValuesContaining { get; set; }
        ConditionsEnum ConditionsEnum { get; set; }
        ushort UshortValueToCompare { get; set; }
        ConditionResultEnum ConditionResult { get; set; }
        Action<ConditionResultChangingEventArgs> ConditionResultChangedAction { get; set; }

       
    }

}