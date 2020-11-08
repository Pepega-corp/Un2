using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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



        public async Task<byte[]> ReadLogicArchive()
        {
            var compressedLogic = await this._fileDriver.ReadFile(LOGARCH_ZIP);
            var uncompressedLogic = UncompressProject(compressedLogic);
            var archLen = uncompressedLogic.Skip(uncompressedLogic.Length - 2).ToArray().ByteArrayToUshortArray()[0];

            if (uncompressedLogic.Length - 2 == archLen)
            {
                return uncompressedLogic;
            }

            return new byte[0];
        }

        public byte[] UncompressProject(byte[] compressedBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(compressedBytes))
            {
                // create a zip
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Read, true))
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
                await provider.WriteSingleRegisterAsync(0x4000, (ushort) pageIndex, "SaveProgrammPage");
                var values = logbin.Skip(pageIndex * 1024).Take(1024).ToArray();
                await provider.WriteMultipleRegistersAsync(0x4300, values, "SaveProgrammBin");
            }
        }

        public async Task WriteStartlogicProgrammSignal()
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            await provider.WriteSingleRegisterAsync(0x0E00, 0x00FF, "StartLogicProgramm");
        }
    }
}
