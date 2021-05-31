using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Conditions;

namespace Unicon2.Fragments.Configuration.Model.Dependencies.Conditions
{
    [JsonObject(MemberSerialization.OptIn)]

    public class CurrentPropertyBitCheckCondition : ICurrentPropertyBitCheckCondition
    {
        [JsonProperty] public ushort BitNumber { get; set; }
    }
}