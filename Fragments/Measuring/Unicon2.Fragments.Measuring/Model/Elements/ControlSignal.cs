using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Model.Address;
using Unicon2.Infrastructure.Connection;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	[JsonObject(MemberSerialization.OptIn)]
    public class ControlSignal : MeasuringElementBase, IControlSignal
    {

        public ControlSignal()
        {
            WritingValueContext = new WritingValueContext();
        }

        public override string StrongName => MeasuringKeys.CONTROL_SIGNAL;

        [JsonProperty]
        public IWritingValueContext WritingValueContext { get; set; }

    }
}
