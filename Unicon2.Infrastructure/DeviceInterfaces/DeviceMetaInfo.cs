using System;
using Newtonsoft.Json;

namespace Unicon2.Infrastructure.DeviceInterfaces
{
    [JsonObject(MemberSerialization.OptIn)]
    public struct DeviceMetaInfo
    {
        [JsonProperty] public Guid? Id { get; set; }
        [JsonProperty] public DateTime? LastEditedDateTime { get; set; }
    }
}