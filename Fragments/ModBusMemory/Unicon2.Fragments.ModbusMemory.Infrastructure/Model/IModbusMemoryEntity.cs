using System;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.Model
{
    public interface IModbusMemoryEntity:ICloneable
    {
        void SetUshortValue(ushort value);
        void SetConversion(IMemoryConversionParameters memoryConversionParameters);
        int Adress { get; set; }
        int DirectValue { get; }
        string ConvertedValue { get; }
    }
}