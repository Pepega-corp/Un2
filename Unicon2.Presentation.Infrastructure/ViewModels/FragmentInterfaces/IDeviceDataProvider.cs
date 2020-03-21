using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IDeviceContextConsumer
    {
        DeviceContext.DeviceContext DeviceContext { get; set; }
    }
}