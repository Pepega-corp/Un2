using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BitGroupValue : IBitGroupValue
    {
        [JsonProperty] public IFormattedValue FormattedValue { get; set; }
        [JsonProperty] public IUshortsFormatter UshortsFormatter { get; set; }
    }
}