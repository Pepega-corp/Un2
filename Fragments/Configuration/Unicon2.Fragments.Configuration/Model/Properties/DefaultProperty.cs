using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Model.Base;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [DataContract(Namespace = "DefaultPropertyNS", Name = nameof(DefaultProperty),IsReference = true)]
    public class DefaultProperty : LocalDeviceValuesConfigurationItemBase, IProperty
    {
        [DataMember(Name =nameof(UshortsFormatter),Order =0)]
        public IUshortsFormatter UshortsFormatter { get; set; }

        [DataMember(Name =nameof(Address),Order =1)]
        
        public ushort Address { get; set; }
        

        [DataMember(Order =2)]
        public ushort NumberOfPoints { get; set; }

        public override string StrongName => nameof(DefaultProperty);


        [DataMember(Order = 3)]
        public string MeasureUnit { get; set; }

        [DataMember(Order = 4)]
        public bool IsMeasureUnitEnabled { get; set; }

        [DataMember(Order = 5)]

        public bool IsRangeEnabled { get; set; }

        [DataMember(Order =6)]
        public IRange Range { get; set; }


        public override async Task Load()
        {
            IQueryResult<ushort[]> queryResult = await _dataProvider.ReadHoldingResgistersAsync(Address, NumberOfPoints,
                  ApplicationGlobalNames.QueriesNames.READING_PROPERTY_QUERY);
            if (queryResult.IsSuccessful)
            {
                DeviceUshortsValue = queryResult.Result;
                await base.Load();
            }
        }


        protected override void FillAddressRanges(List<IRange> ranges)
        {
            IRange range = _rangeGetFunc();
            range.RangeFrom = this.Address;
            range.RangeTo = this.Address + NumberOfPoints;
            ranges.Add(range);
        }

        public override async Task<bool> Write()
        {
            if (IsValuesEqual) return false;
            if (LocalUshortsValue == null) return false;
            IQueryResult queryResult = await _dataProvider.WriteMultipleRegistersAsync(Address, LocalUshortsValue, ApplicationGlobalNames.QueriesNames.WRITE_CONFIGURATION_QUERY_KEY);
            if (queryResult.IsSuccessful)
            {
                return await base.Write();
            }
            return false;
        }



        public override void TransferDeviceLocalData(bool isFromDeviceToLocal)
        {
            if (isFromDeviceToLocal)
            {
                if (DeviceUshortsValue == null) return;
                LocalUshortsValue = DeviceUshortsValue.Clone() as ushort[];
            }
            else
            {
                if (LocalUshortsValue == null) return;
                DeviceUshortsValue = LocalUshortsValue.Clone() as ushort[];
            }
            base.TransferDeviceLocalData(isFromDeviceToLocal);
        }



        public override void InitializeLocalValue(IConfigurationItem localConfigurationItem)
        {
            if (localConfigurationItem is DefaultProperty)
            {
                LocalUshortsValue = (localConfigurationItem as DefaultProperty).LocalUshortsValue;
            }
            base.InitializeLocalValue(localConfigurationItem);
        }

        public override void InitializeValue(IConfigurationItem localConfigurationItem)
        {
            base.InitializeValue(localConfigurationItem);
        }


        protected override IConfigurationItem OnCloning()
        {
            DefaultProperty cloneProperty = new DefaultProperty(_rangeGetFunc);
            cloneProperty.UshortsFormatter = UshortsFormatter;
            cloneProperty.Address = Address;
            cloneProperty.NumberOfPoints = NumberOfPoints;
            cloneProperty.MeasureUnit = MeasureUnit;
            cloneProperty.IsMeasureUnitEnabled = IsMeasureUnitEnabled;
            cloneProperty.Range = Range.Clone() as IRange;
            cloneProperty.IsRangeEnabled = IsRangeEnabled;
            return cloneProperty;
        }


        public override void InitializeFromContainer(ITypesContainer container)
        {
            (this.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(container);
            base.InitializeFromContainer(container);
        }


        public DefaultProperty(Func<IRange> rangeGetFunc) : base(rangeGetFunc)
        {
        }
    }
}