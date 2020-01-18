using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileDataReader : IFileDataReader
    {
        private IDataProvider _dataProvider;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task<byte[]> GetDataBytes(int dataLenght = 256)
        {
            List<ushort> ushorts = new List<ushort>();
            ushort iterator = 64;
            int diff = (dataLenght % 2);
            int ushortLenght = dataLenght / 2 + diff;
            if (ushortLenght < 64)
            {
                iterator = (ushort)ushortLenght;
            }
            for (ushort i = 0; i < ushortLenght; i += iterator)
            {
                IQueryResult<ushort[]> ushortQueryResult = await this._dataProvider.ReadHoldingResgistersAsync((ushort)(0x5200 + i), iterator, "ReadStateCmdFileDriver");
                ushorts.AddRange(ushortQueryResult.Result);
            }
            if (diff == 0)
            {
                return ushorts.ToArray().UshortArrayToByteArray(false);
            }
            else
            {
                return ushorts.ToArray().UshortArrayToByteArray(false).Take(dataLenght).ToArray();
            }
        }
    }
}
