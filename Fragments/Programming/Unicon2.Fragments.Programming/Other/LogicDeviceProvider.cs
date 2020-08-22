using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.Programming.Model;
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

        public void WriteLogic(byte[] logicPjectBytes)
        {
            var compressedArchive = this.CompressProject(logicPjectBytes);
        }

        private ushort[] CompressProject(byte[] logicArchive)
        {
            ushort[] compressedWords = null;

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

                var compressedBytes = memoryStream.ToArray();
                compressedWords = new ushort[(compressedBytes.Length + 1) / 2 + 3]; //Размер хранилища
                compressedWords[0] = (ushort)logicArchive.Length; // размер несжатого проекта (байты)
                compressedWords[1] = 0x0001; // Версия упаковщика
                compressedWords[2] = 0x0000; // СRС архива проекта
                for (int i = 3; i < compressedWords.Length; i++)
                {
                    if ((i - 2) * 2 + 1 < compressedBytes.Length)
                    {
                        compressedWords[i] = (ushort)(compressedBytes[(i - 3) * 2 + 1] << 8);
                    }
                    else
                    {
                        compressedWords[i] = 0;
                    }
                    compressedWords[i] += compressedBytes[(i - 3) * 2];
                }
            }

            return compressedWords;
        }

        public async Task<byte[]> ReadLogic()
        {
            var compressedLogic = await this._fileDriver.ReadFile(LOGARCH_ZIP);

            return new byte[] { };
        }

        public byte[] UncompressProject(ushort[] compressedWords)
        {
            byte[] compressedBytes = new byte[(compressedWords.Length - 3) * 2];
            for (int i = 3; i < compressedWords.Length; i++)
            {
                if ((i - 2) * 2 + 1 < compressedBytes.Length)
                {
                    compressedBytes[(i - 3) * 2 + 1] = (byte)(compressedWords[i] >> 8);
                }
                compressedBytes[(i - 3) * 2] = (byte)compressedWords[i];
            }

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
    }
}
