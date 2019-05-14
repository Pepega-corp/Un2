using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Formatting.Infrastructure.Model
{
    public interface IFormulaFormatter : IUshortsFormatter, IInitializableFromContainer
    {
        string FormulaString { get; set; }
        List<IUshortFormattable> UshortFormattables { get; set; }
        ushort NumberOfSimbolsAfterComma { get; set; }
    }
}