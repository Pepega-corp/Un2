using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Model
{
   public class ConnectionContainer:IDataProviderContainer
    {
        public ConnectionContainer(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }
        public IDataProvider DataProvider { get; set; }
    }
}
