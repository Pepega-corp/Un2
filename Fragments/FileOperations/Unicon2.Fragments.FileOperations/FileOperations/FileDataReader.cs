using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileDataReader : IFileDataReader
    {
        public DeviceContext DeviceContext { get; set; }

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

                var res = await this.DeviceContext.DataProviderContainer.DataProvider.OnSuccessAsync(
                    async (provider) =>
                        Result<IQueryResult<ushort[]>>.Create(
                            await provider.ReadHoldingResgistersAsync((ushort)(0x5200 + i), iterator, "ReadStateCmdFileDriver"),
                            true));
//todo add check
                ushorts.AddRange(res.Item.Result);
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
