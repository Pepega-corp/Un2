using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IDeviceDataProvider
    {
        void SetDeviceData(string deviceName);
        string GetDeviceName();
    }
}