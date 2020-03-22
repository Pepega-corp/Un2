using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	[JsonObject(MemberSerialization.OptIn)]
	public class AnalogMeasuringElement : MeasuringElementBase, IAnalogMeasuringElement
	{
		public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT;
		[JsonProperty] public ushort Address { get; set; }
		[JsonProperty] public ushort NumberOfPoints { get; set; }

		[JsonProperty] public IUshortsFormatter UshortsFormatter { get; set; }
		[JsonProperty] public string MeasureUnit { get; set; }
		[JsonProperty] public bool IsMeasureUnitEnabled { get; set; }

		public object Clone()
		{
			return new AnalogMeasuringElement()
			{
				Address=Address,
				NumberOfPoints= NumberOfPoints,
				UshortsFormatter = UshortsFormatter,
				MeasureUnit = MeasureUnit,
				IsMeasureUnitEnabled = IsMeasureUnitEnabled
			};
		}
	}
}
