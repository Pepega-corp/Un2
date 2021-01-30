using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.Programming.Infrastructure.HelperClasses;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Programming.Other
{
    public class LogicDeviceProvider
    {
        private const string LOGARCH_ZIP = "logarch.zip";
        private readonly IFileDriver _fileDriver;
        private Compressor _compressor;

        public DeviceContext DeviceContext { get; private set; }

        public LogicDeviceProvider(IFileDriver fileDriver)
        {
            this._fileDriver = fileDriver;
            _compressor = new Compressor();
        }

        public void SetDeviceContext(DeviceContext deviceContext)
        {
            this.DeviceContext = deviceContext;
            this._fileDriver.SetDataProvider(deviceContext.DataProviderContainer.DataProvider);
        }

        public async Task WriteLogicArchive(byte[] logicPjectBytes, bool hasFileSystem)
        {
            var compressedArchive = _compressor.Compress(logicPjectBytes);
            if (hasFileSystem)
            {
                await this._fileDriver.WriteFile(compressedArchive, LOGARCH_ZIP);
            }
            else
            {
                
            }
        }
        public async Task<byte[]> ReadLogicArchive(bool hasFileSystem)
        {
            byte[] uncompressedLogic = new byte[0];
            if (hasFileSystem)
            {
                var compressedArchive = new List<byte>(await this._fileDriver.ReadFile(LOGARCH_ZIP));
                var lenght = Extensions.ToUshort(compressedArchive[compressedArchive.Count - 1], compressedArchive[compressedArchive.Count - 2]);
                                if (lenght != compressedArchive.Count - 2)
                {
                    throw new Exception("Logic archive has invalid bytes lenght");
                }
                compressedArchive.RemoveRange(compressedArchive.Count-2, 2);
                uncompressedLogic =_compressor.Decompress(compressedArchive.ToArray());
                var archLen = uncompressedLogic.Skip(uncompressedLogic.Length - 2).ToArray()
                    .ByteArrayToUshortArray()[0];
                if (uncompressedLogic.Length - 2 == archLen)
                {
                    return uncompressedLogic;
                }
            }
            else
            {
                
            }

            return uncompressedLogic;
        }

        public async Task WriteLogicProgrammBin(ushort[] logbin)
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            for (int pageIndex = 0; pageIndex < 4; pageIndex++)
            {
                var pageSaveResult = await provider.WriteSingleRegisterAsync(0x4000, (ushort) pageIndex, "SaveProgrammPage");
                if (pageSaveResult.IsSuccessful)
                {
                    var values = logbin.Skip(pageIndex * 1024).Take(1024).ToArray();
                    var count = 1024 / 64;
                    for (var i = 0; i < count; i++)
                    {
                        var val = values.Skip(i * 64).Take(64).ToArray();
                        var programmSaveResult = await provider.WriteMultipleRegistersAsync((ushort)(0x4300 + i*64), val, $"SaveProgrammBin{i}");
                        if (!programmSaveResult.IsSuccessful)
                        {
                            throw new Exception("Can't write logic programm in device");
                        }
                    }
                }
                else
                {
                    throw new Exception("Can't write logic programm page in device");
                }
            }
        }

        public async Task WriteStartlogicProgrammSignal()
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            var result = await provider.WriteSingleCoilAsync(0x0E00, true, "StartLogicProgramm");
            if (!result.IsSuccessful)
            {
                throw new Exception("Can't start logic programm");
            }
        }

        public async Task<ushort[]> ReadConnectionValues(int connectionsCount)
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            var response = await provider.ReadHoldingResgistersAsync(0x4100, (ushort)connectionsCount, "ReadLogProgSignals");
            if (response.IsSuccessful)
            {
                return response.Result;
            }

            throw new Exception("Can't read connections values");
        }
    }
}
