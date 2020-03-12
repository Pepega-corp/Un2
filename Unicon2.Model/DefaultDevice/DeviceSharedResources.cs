using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Model.DefaultDevice
{
    [DataContract(Name = nameof(DeviceSharedResources), Namespace = "DeviceSharedResourcesNS")]
    public class DeviceSharedResources : Disposable, IDeviceSharedResources
    {
        public DeviceSharedResources()
        {
            this.SharedResources = new List<INameable>();
        }

        [DataMember(Name = nameof(SharedResources))]
        public List<INameable> SharedResources { get; set; }

    }
}