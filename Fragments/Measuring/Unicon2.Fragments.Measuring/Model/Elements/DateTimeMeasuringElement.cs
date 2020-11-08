using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DateTimeMeasuringElement : MeasuringElementBase, IDateTimeMeasuringElement
	{
	    public override string StrongName => MeasuringKeys.DATE_TIME_ELEMENT;
	    [JsonProperty]

        public ushort StartAddress { get; set; }
	}
}