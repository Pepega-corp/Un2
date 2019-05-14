using System;
using System.Collections;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.DependentProperty;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;
using Unicon2.Infrastructure.Interfaces.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.DependentProperty
{
    [DataContract(Namespace = "DependancyConditionNS")]

    public class DependancyCondition : IDependancyCondition
    {
        //private ushort[] _previousDeviceUshorts;
        //private ushort[] _previousLocalUshorts;

        #region Implementation of IResourceContaining

        private void CheckReferencedProperty(bool isLocal)
        {
            if (this.ConditionsEnum == ConditionsEnum.HaveTrueBitAt && this.LocalAndDeviceValuesContaining.DeviceUshortsValue != null)
            {
                if (isLocal)
                {
                    BitArray bitArray = new BitArray(new int[] {this.LocalAndDeviceValuesContaining.LocalUshortsValue[0]});
                    bool isConditionTriggered = bitArray[this.UshortValueToCompare];
                    this.ConditionResultChangedAction?.Invoke(new ConditionResultChangingEventArgs()
                    {
                        ConditionResult = this.ConditionResult,
                        IsConditionTriggered = isConditionTriggered,
                        IsLocalValueTriggered = true
                    });
                }
                else
                {
                    BitArray bitArray = new BitArray(new int[]{this.LocalAndDeviceValuesContaining.DeviceUshortsValue[0]});
                    bool isConditionTriggered = bitArray[this.UshortValueToCompare];
                    this.ConditionResultChangedAction?.Invoke(new ConditionResultChangingEventArgs
                    {
                        ConditionResult = this.ConditionResult,
                        IsConditionTriggered = isConditionTriggered,
                        IsLocalValueTriggered = false
                    });
                }
            }
        }

        #endregion


        #region Implementation of IDependancyCondition

        public ILocalAndDeviceValuesContaining LocalAndDeviceValuesContaining { get; set; }
        [DataMember(Order = 3)]
        public ConditionsEnum ConditionsEnum { get; set; }
        [DataMember(Order = 4)]
        public ushort UshortValueToCompare { get; set; }
        [DataMember(Order = 5)]
        public ConditionResultEnum ConditionResult { get; set; }

        public Action<ConditionResultChangingEventArgs> ConditionResultChangedAction { get; set; }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => ConfigurationKeys.DEPENDANCY_CONDITION;

        #endregion

        #region Implementation of INameable
        [DataMember(Order = 6)]
        public string Name { get; set; }

        #endregion

        #region Implementation of IUshortFormattable
        [DataMember(Order = 7)]
        public IUshortsFormatter UshortsFormatter { get; set; }

        #endregion



        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (!this.IsInitialized)
            {
                if (this.UshortsFormatter is IInitializableFromContainer)
                {
                    (this.UshortsFormatter as IInitializableFromContainer).InitializeFromContainer(container);
                }
            }
            this.IsInitialized = true;
        }

        #endregion

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            if (this.LocalAndDeviceValuesContaining == null) return;
            this.LocalAndDeviceValuesContaining.DeviceUshortsValueChanged += () => this.CheckReferencedProperty(false);
            this.LocalAndDeviceValuesContaining.LocalUshortsValueChanged += () => this.CheckReferencedProperty(true);
        }
    }
}
