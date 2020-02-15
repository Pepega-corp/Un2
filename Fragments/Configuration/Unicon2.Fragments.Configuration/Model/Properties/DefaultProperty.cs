using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [DataContract(Namespace = "DefaultPropertyNS", Name = nameof(DefaultProperty),IsReference = true)]
    public class DefaultProperty : ConfigurationItemBase, IProperty
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

        protected override IConfigurationItem OnCloning()
        {
            DefaultProperty cloneProperty = new DefaultProperty();
            cloneProperty.UshortsFormatter = UshortsFormatter;
            cloneProperty.Address = Address;
            cloneProperty.NumberOfPoints = NumberOfPoints;
            cloneProperty.MeasureUnit = MeasureUnit;
            cloneProperty.IsMeasureUnitEnabled = IsMeasureUnitEnabled;
            cloneProperty.Range = Range.Clone() as IRange;
            cloneProperty.IsRangeEnabled = IsRangeEnabled;
            return cloneProperty;
        }
        
    }
}