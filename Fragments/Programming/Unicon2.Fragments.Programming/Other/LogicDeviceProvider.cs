using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Programming.Other
{
    public class LogicDeviceProvider
    {
        private const string LOGARCH_ZIP = "logarch.zip";
        private readonly IFileDriver _fileDriver;

        public DeviceContext DeviceContext { get; private set; }

        public LogicDeviceProvider(IFileDriver fileDriver)
        {
            this._fileDriver = fileDriver;
        }

        public void SetDeviceContext(DeviceContext deviceContext)
        {
            this.DeviceContext = deviceContext;
            this._fileDriver.SetDataProvider(deviceContext.DataProviderContainer.DataProvider);
        }

        public async Task WriteLogicArchive(byte[] logicPjectBytes, bool hasFileSystem)
        {
            var compressedArchive = this.CompressProject(logicPjectBytes);
            if (hasFileSystem)
            {
                await this._fileDriver.WriteFile(compressedArchive, LOGARCH_ZIP);
            }
            else
            {
                
            }
        }

        private byte[] CompressProject(byte[] logicArchive)
        {
            var compressedBytes = new List<byte>();

            // create a working memory stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // create a zip
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    // add the item name to the zip
                    ZipArchiveEntry zipItem = zip.CreateEntry("logicarchive" + ProgramModel.EXTENSION);
                    // add the item bytes to the zip entry by opening the original file and copying the bytes
                    using (MemoryStream originalFileMemoryStream = new MemoryStream(logicArchive))
                    {
                        using (Stream entryStream = zipItem.Open())
                        {
                            originalFileMemoryStream.CopyTo(entryStream);
                        }
                    }
                }

                compressedBytes.AddRange(memoryStream.ToArray());
                var archLen = (ushort)compressedBytes.Count;
                //Add archive length
                compressedBytes.AddRange(archLen.UshortToBytes());
            }

            return compressedBytes.ToArray();
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
                uncompressedLogic = UncompressProject(compressedArchive.ToArray());
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

        public byte[] UncompressProject(byte[] compressedBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                // create a zip
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Update, true))
                {
                    // add the item name to the zip
                    ZipArchiveEntry zipItem = zip.GetEntry("logicarchive" + ProgramModel.EXTENSION);
                    // add the item bytes to the zip entry by opening the original file and copying the bytes
                    using (MemoryStream uncompressedStream = new MemoryStream())
                    {
                        using (Stream entryStream = zipItem.Open())
                        {
                            entryStream.CopyTo(uncompressedStream);
                        }

                        return uncompressedStream.ToArray();
                    }
                }
            }
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
