using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;

namespace Unicon2.Fragments.ModbusMemory.Model
{
    public class ModbusMemoryEntity : IModbusMemoryEntity
    {
        private ushort _value;
        private IMemoryConversionParameters _memoryConversionParameters;


        public void SetUshortValue(ushort value)
        {
            this._value = value;
        }

        public void SetConversion(IMemoryConversionParameters memoryConversionParameters)
        {
            this._memoryConversionParameters = memoryConversionParameters;
        }

        public int Adress { get; set; }

        public int DirectValue => this._value;

        public string ConvertedValue => ((this._memoryConversionParameters.LimitOfValue * this._value) /
                                         this._memoryConversionParameters.MaximumOfUshortValue)
            .ToString("N" + this._memoryConversionParameters.NumberOfSigns);


        public object Clone()
        {
            IModbusMemoryEntity clone = new ModbusMemoryEntity();
            clone.Adress = this.Adress;
            clone.SetUshortValue(this._value);
            clone.SetConversion(this._memoryConversionParameters);
            return clone;
        }
    }
}