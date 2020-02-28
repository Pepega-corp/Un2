using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Presentation.Infrastructure.Subscription;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IDeviceDataProvider
    {
        void SetDeviceData(string deviceName, IDeviceEventsDispatcher deviceEventsDispatcher);
        string GetDeviceName();
    }
}