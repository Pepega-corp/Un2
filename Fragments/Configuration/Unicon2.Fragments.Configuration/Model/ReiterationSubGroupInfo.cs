using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ReiterationSubGroupInfo : IReiterationSubGroupInfo
    {

        [JsonProperty] public string Name { get; set; }

        public object Clone()
        {
            return new ReiterationSubGroupInfo()
            {
                Name = Name,
            };
        }
    }
}