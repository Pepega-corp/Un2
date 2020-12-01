using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GroupWithReiterationInfo : IGroupWithReiterationInfo
    {
        [JsonProperty] public int ReiterationStep { get; set; }

        [JsonProperty] public List<IReiterationSubGroupInfo> SubGroups { get; set; }

        [JsonProperty] public bool IsReiterationEnabled { get; set; }

        public object Clone()
        {
            var clone = new GroupWithReiterationInfo
            {
                IsReiterationEnabled = IsReiterationEnabled,
                ReiterationStep = ReiterationStep,
                SubGroups = SubGroups.Select((info => info.Clone() as IReiterationSubGroupInfo)).ToList()
            };
            return clone;
        }
    }
}