using System;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IUshortsFormatter : IStronglyNamed, ICloneable, INameable
    {
        IFormattedValue Format(ushort[] ushorts);
        ushort[] FormatBack(IFormattedValue formattedValue);
    }
}