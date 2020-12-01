using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [JsonObject(MemberSerialization.OptIn)]
    public class JournalParameter : IJournalParameter
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public IUshortsFormatter UshortsFormatter { get; set; }
        [JsonProperty] public string MeasureUnit { get; set; }
        [JsonProperty] public bool IsMeasureUnitEnabled { get; set; }
        [JsonProperty] public ushort StartAddress { get; set; }
        [JsonProperty] public ushort NumberOfPoints { get; set; }
    
        public object Clone()
        {
            return new JournalParameter()
            {

            };
        }
    }
}