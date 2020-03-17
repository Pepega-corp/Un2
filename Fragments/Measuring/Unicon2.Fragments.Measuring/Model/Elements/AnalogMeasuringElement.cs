using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	[DataContract(Namespace = "AnalogMeasuringElementNS")]
	public class AnalogMeasuringElement : MeasuringElementBase, IAnalogMeasuringElement

	{
		public override string StrongName => MeasuringKeys.ANALOG_MEASURING_ELEMENT;
		[DataMember] public ushort Address { get; set; }
		[DataMember] public ushort NumberOfPoints { get; set; }

		[DataMember] public IUshortsFormatter UshortsFormatter { get; set; }
		[DataMember] public string MeasureUnit { get; set; }
		[DataMember] public bool IsMeasureUnitEnabled { get; set; }

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
