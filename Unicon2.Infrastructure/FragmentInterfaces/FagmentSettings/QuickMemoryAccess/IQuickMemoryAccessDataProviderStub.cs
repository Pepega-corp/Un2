using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
   public interface IQuickMemoryAccessDataProviderStub:IDataProvider,IDataProviderContaining
    {
        Task<IQueryResult> WriteMultipleRegistersByBitNumbersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle,List<int> bitNumbers);
    }
}
