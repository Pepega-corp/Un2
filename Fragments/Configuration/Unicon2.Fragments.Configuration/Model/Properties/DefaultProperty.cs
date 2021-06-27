using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Dependencies;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Configuration.Model.Properties
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultProperty : ConfigurationItemBase, IProperty
    {
        [JsonProperty] public IUshortsFormatter UshortsFormatter { get; set; }
        [JsonProperty] public bool IsFromBits { get; set; }
        [JsonProperty] public List<ushort> BitNumbers { get; set; } = new List<ushort>();

        [JsonProperty] public ushort Address { get; set; } = 1;
        [JsonProperty] public ushort NumberOfPoints { get; set; } = 1;

        [DefaultValue(16)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public ushort NumberOfWriteFunction { get; set; }

        [JsonProperty] public string MeasureUnit { get; set; }

        [JsonProperty] public bool IsMeasureUnitEnabled { get; set; }

        [JsonProperty] public bool IsRangeEnabled { get; set; }

        [JsonProperty] public IRange Range { get; set; }
        [JsonProperty] public List<IDependency> Dependencies { get; set; } = new List<IDependency>();
        [JsonProperty] public bool IsHidden { get; set; }

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
            cloneProperty.NumberOfWriteFunction = NumberOfWriteFunction;
            cloneProperty.IsHidden = IsHidden;

            return cloneProperty;
        }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitProperty(this);
        }
    }
}