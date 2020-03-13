using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
	[JsonObject(MemberSerialization.OptIn)]

	public class NumericValue : FormattedValueBase, INumericValue
	{
		[JsonProperty] public string MeasureUnit { get; set; }
		[JsonProperty] public bool IsMeasureUnitEnabled { get; set; }

		public override string StrongName => nameof(NumericValue);

		public override string AsString()
		{
			return ToString();
		}

		[JsonProperty] public double NumValue { get; set; }

		public override string ToString()
		{
			return NumValue.ToString();
		}

		public override T Accept<T>(IValueVisitor<T> visitor)
		{
			return visitor.VisitNumericValue(this);
		}
	}
}
