using System;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Configuration.Model.DependentProperty
{
    [DataContract(Namespace = "DependancyConditionNS")]

    public class DependancyCondition : IDependancyCondition
    {

        //private void CheckReferencedProperty(bool isLocal)
        //{
        //    if (this.ConditionsEnum == ConditionsEnum.HaveTrueBitAt && this.LocalAndDeviceValuesContaining.DeviceUshortsValue != null)
        //    {
        //        if (isLocal)
        //        {
        //            BitArray bitArray = new BitArray(new int[] {this.LocalAndDeviceValuesContaining.LocalUshortsValue[0]});
        //            bool isConditionTriggered = bitArray[this.UshortValueToCompare];
        //            this.ConditionResultChangedAction?.Invoke(new ConditionResultChangingEventArgs()
        //            {
        //                ConditionResult = this.ConditionResult,
        //                IsConditionTriggered = isConditionTriggered,
        //                IsLocalValueTriggered = true
        //            });
        //        }
        //        else
        //        {
        //            BitArray bitArray = new BitArray(new int[]{this.LocalAndDeviceValuesContaining.DeviceUshortsValue[0]});
        //            bool isConditionTriggered = bitArray[this.UshortValueToCompare];
        //            this.ConditionResultChangedAction?.Invoke(new ConditionResultChangingEventArgs
        //            {
        //                ConditionResult = this.ConditionResult,
        //                IsConditionTriggered = isConditionTriggered,
        //                IsLocalValueTriggered = false
        //            });
        //        }
        //    }
        //}
        
        [DataMember(Order = 3)]
        public ConditionsEnum ConditionsEnum { get; set; }
        [DataMember(Order = 4)]
        public ushort UshortValueToCompare { get; set; }
        [DataMember(Order = 5)]
        public ConditionResultEnum ConditionResult { get; set; }

        public Action<ConditionResultChangingEventArgs> ConditionResultChangedAction { get; set; }

        public string StrongName => ConfigurationKeys.DEPENDANCY_CONDITION;

        [DataMember(Order = 6)]
        public string Name { get; set; }

        [DataMember(Order = 7)]
        public IUshortsFormatter UshortsFormatter { get; set; }


        public bool IsInitialized { get; private set; }
        
        //[OnDeserialized]
        //private void OnDeserialized(StreamingContext sc)
        //{
        //    if (this.LocalAndDeviceValuesContaining == null) return;
        //    this.LocalAndDeviceValuesContaining.DeviceUshortsValueChanged += () => this.CheckReferencedProperty(false);
        //    this.LocalAndDeviceValuesContaining.LocalUshortsValueChanged += () => this.CheckReferencedProperty(true);
        //}
        public object Clone()
        {
            var cond=new DependancyCondition();
            cond.Name = Name;
            cond.ConditionResult = ConditionResult;
            cond.ConditionsEnum = ConditionsEnum;
            cond.UshortValueToCompare = UshortValueToCompare;
            cond.UshortsFormatter = UshortsFormatter;
            return cond;
        }
    }
}
