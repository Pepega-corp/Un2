using System;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Model.Properties;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Values;

namespace Unicon2.Fragments.Configuration.Model.Base
{
    [DataContract(Namespace = "LocalDeviceValuesConfigurationItemBaseNS", IsReference = true), KnownType(typeof(DefaultProperty))]

    public abstract class LocalDeviceValuesConfigurationItemBase : ConfigurationItemBase, ILocalAndDeviceValuesContaining
    {
        private ushort[] _deviceUshortsValue;
        private ushort[] _localUshortsValue;

        protected LocalDeviceValuesConfigurationItemBase(Func<IRange> rangeGetFunc) : base(rangeGetFunc)
        {
        }

        public ushort[] DeviceUshortsValue
        {
            get { return this._deviceUshortsValue; }
            set
            {
                if (this._deviceUshortsValue.CheckEquality(value)) return;
                this._deviceUshortsValue = value;
                this.DeviceUshortsValueChanged?.Invoke();
            }
        }

        [DataMember]
        public ushort[] LocalUshortsValue
        {
            get { return this._localUshortsValue; }
            set
            {
                if (this._localUshortsValue.CheckEquality(value)) return;
                this._localUshortsValue = value;
                this.LocalUshortsValueChanged?.Invoke();
            }
        }
        public Action DeviceUshortsValueChanged { get; set; }
        public Action LocalUshortsValueChanged { get; set; }
        public bool IsValuesEqual => this.LocalUshortsValue.CheckEquality(this.DeviceUshortsValue);
    }
}
