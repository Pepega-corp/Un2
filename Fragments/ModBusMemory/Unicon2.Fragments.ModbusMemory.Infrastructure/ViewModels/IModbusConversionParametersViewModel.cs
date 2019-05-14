using System;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels
{
    public interface IModbusConversionParametersViewModel
    {
        IMemoryConversionParameters GetConversionParameters();
        int LimitOfValue { get;set; }
        int MaximumOfUshortValue { get; set; }
        int NumberOfSigns { get; set; }
        Action<IMemoryConversionParameters> MemoryConversionParametersChanged { get; set; }
    }
}