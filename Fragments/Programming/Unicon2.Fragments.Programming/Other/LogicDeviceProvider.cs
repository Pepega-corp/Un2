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
        private const ushort LOGIC_BIN_START_ADDRESS_NEW_DEVICES = 0x4300; // addresses move to editor!!!
        private const ushort LOGIC_BIN_PAGE_ADDRESS = 0x4000; // addresses move to editor!!!
        private const ushort LOGIC_SYGNAL_START_ADDRESS_NEW_DEVICES = 0x4100; // addresses move to editor!!!
        private const ushort START_LOGIC_ADDRESS_NEW_DEVICE = 0x0E00; // addresses move to editor!!!

        private const ushort LOGIC_BIN_START_ADDRESS_OLD_DEVICES = 0xB000; // addresses move to editor!!!
        private const ushort LOGIC_ARCH_START_ADDRESS_OLD_DEVICES = 0xC000; // addresses move to editor!!! 
        private const ushort LOGIC_SYGNAL_START_ADDRESS_OLD_DEVICES = 0xA000; // addresses move to editor!!!
        private const ushort START_LOGIC_ADDRESS_OLD_DEVICE = 0x0001; // addresses move to editor!!!

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

        public async Task WriteLogicArchive(byte[] logicPjectBytes, bool hasFileSystem, ushort writeDataLen = 64)
        {
            var compressedArchive = _compressor.Compress(logicPjectBytes);
            if (hasFileSystem)
            {
                await this._fileDriver.WriteFile(compressedArchive, LOGARCH_ZIP, writeDataLen);
            }
            else
            {
                await Task.Delay(100);
                var writeArchData = new ushort[8192];
                var logicProjectWords = compressedArchive.ByteArrayToUshortArray();
                logicProjectWords.CopyTo(writeArchData, 0);
                var isWrote = await Write(writeArchData, LOGIC_ARCH_START_ADDRESS_OLD_DEVICES, "WriteLogicArchive", writeDataLen);
                if (!isWrote)
                {
                    throw new Exception("Can't write logic programm archive in device");
                }
            }
        }

        public async Task WriteLogicProgrammBin(ushort[] logbin, bool hasFileSystem, ushort writeDataLen = 64)
        {
            var pageCount = hasFileSystem ? 4 : 1;
            ushort writeStartAddress = hasFileSystem ? LOGIC_BIN_START_ADDRESS_NEW_DEVICES :LOGIC_BIN_START_ADDRESS_OLD_DEVICES;

            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                bool isSuccessful;
                if (hasFileSystem)
                {
                    var pageSaveResult = await this.DeviceContext.DataProviderContainer.DataProvider
                        .WriteSingleRegisterAsync(LOGIC_BIN_PAGE_ADDRESS, (ushort)pageIndex, "SaveProgrammPage");
                    isSuccessful = pageSaveResult.IsSuccessful;
                }
                else
                {
                    isSuccessful = true;
                }

                if (isSuccessful)
                {
                    var values = logbin.Skip(pageIndex * 1024).Take(1024).ToArray();
                    var isWrote = await Write(values, writeStartAddress, "WriteProgrammBin", writeDataLen);
                    if (!isWrote)
                    {
                        throw new Exception("Can't write logic programm bin in device");
                    }
                }
                else
                {
                    throw new Exception("Can't write logic programm page in device");
                }
            }
        }

        private async Task<bool> Write(ushort[] values, ushort startAddress, string writeQueryName, ushort writeDataLen)
        {
            var count = values.Length / writeDataLen;
            for (var i = 0; i < count; i++)
            {
                var val = values.Skip(i * writeDataLen).Take(writeDataLen).ToArray();
                var programmSaveResult = await this.DeviceContext.DataProviderContainer.DataProvider
                    .WriteMultipleRegistersAsync((ushort)(startAddress + i * writeDataLen), val, $"{writeQueryName}{i}");
                if (!programmSaveResult.IsSuccessful)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task WriteStartlogicProgrammSignal(bool hasFileSystem)
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            var startAddress = hasFileSystem ? START_LOGIC_ADDRESS_NEW_DEVICE : START_LOGIC_ADDRESS_OLD_DEVICE;
            var result = await provider.WriteSingleCoilAsync(startAddress, true, "StartLogicProgramm");
            if (!result.IsSuccessful)
            {
                throw new Exception("Can't start logic programm");
            }
        }

        public async Task<ushort[]> ReadConnectionValues(int connectionsCount, bool hasFileSystem)
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            var startAddress = hasFileSystem ? LOGIC_SYGNAL_START_ADDRESS_NEW_DEVICES : LOGIC_SYGNAL_START_ADDRESS_OLD_DEVICES;
            var response = await provider.ReadHoldingResgistersAsync(startAddress, (ushort)connectionsCount, "ReadLogProgSignals");
            if (response.IsSuccessful)
            {
                return response.Result;
            }

            throw new Exception("Can't read connections values");
        }

        public async Task<byte[]> ReadLogicArchive(bool hasFileSystem, ushort readDataLen = 64)
        {
            byte[] compressedLogicArchive;

            if (hasFileSystem)
            {
                compressedLogicArchive = await this._fileDriver.ReadFile(LOGARCH_ZIP);
            }
            else
            {
                compressedLogicArchive = await ReadLogicArchive(64);
            }

            return _compressor.Decompress(compressedLogicArchive);
        }

        private async Task<byte[]> ReadLogicArchive(ushort readDataLen)
        {
            var provider = this.DeviceContext.DataProviderContainer.DataProvider;
            var count = 8192 / readDataLen; // storage size move to editor!!!
            var residue = 8192 % readDataLen;

            var logicArchive = new List<ushort>();
            var address = LOGIC_ARCH_START_ADDRESS_OLD_DEVICES;
            var i = 0;
            for (; i < count; i++)
            {
                var result = await provider.ReadHoldingResgistersAsync(address, readDataLen, $"ReadLogicArcive{i}");
                if (result.IsSuccessful)
                {
                    logicArchive.AddRange(result.Result);
                }
                else
                {
                    throw new Exception("Can't read logic archive");
                }
                address += (ushort)((i+1) * readDataLen);
            }
            if(residue > 0)
            {
                var result = await provider.ReadHoldingResgistersAsync(address, (ushort)residue, $"ReadLogicArcive{i}");
                if (result.IsSuccessful)
                {
                    logicArchive.AddRange(result.Result);
                }
                else
                {
                    throw new Exception("Can't read logic archive");
                }
            }

            return logicArchive.UshortListToByteArray(true);
        }
    }
}
