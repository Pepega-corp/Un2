using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Model.DefaultDevice
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DeviceSharedResources : Disposable, IDeviceSharedResources
    {
        public DeviceSharedResources()
        {
            SharedResources = new List<INameable>();
        }

        [JsonProperty]
        public List<INameable> SharedResources { get; set; }

    }
}