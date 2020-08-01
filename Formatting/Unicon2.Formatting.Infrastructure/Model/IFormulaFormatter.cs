using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface IFormulaFormatter : IUshortsFormatter
    {
        string FormulaString { get; set; }
        List<string> UshortFormattableResources { get; set; }
        ushort NumberOfSimbolsAfterComma { get; set; }
    }
}