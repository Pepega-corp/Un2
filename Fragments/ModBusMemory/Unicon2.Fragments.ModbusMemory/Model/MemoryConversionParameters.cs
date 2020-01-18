using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;

namespace Unicon2.Fragments.ModbusMemory.Model
{
    public class MemoryConversionParameters : IMemoryConversionParameters
    {
        public MemoryConversionParameters()
        {
            this.MaximumOfUshortValue = ushort.MaxValue;
            this.NumberOfSigns = 1;
            this.LimitOfValue = 100;
        }

        public int LimitOfValue { get; set; }

        public int MaximumOfUshortValue { get; set; }

        public int NumberOfSigns { get; set; }
    }
}
