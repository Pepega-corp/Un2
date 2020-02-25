using System;

namespace Unicon2.Infrastructure.Interfaces
{
    public interface IRange : ICloneable
    {
        double RangeFrom { get; set; }
        double RangeTo { get; set; }
        bool CheckValue(double valueToCheck);
        bool CheckNesting(IRange range);
    }
}